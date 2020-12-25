using EGameCafe.Application.Common.Exceptions;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models;
using EGameCafe.Application.Models.Identity;
using EGameCafe.Domain.Entities;
using EGameCafe.Infrastructure.PreparedResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationDbContext _context;
        private readonly IMobileSenders _mobileSenders;
        private readonly IEmailSender _emailSender;
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<JWTKeys> _jwtKeys;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationDbContext applicationDbContext,
            IMobileSenders mobileSenders,
            IEmailSender emailSender,
            IMemoryCache memoryCache,
            IOptions<JWTKeys> jwtKeys
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = applicationDbContext;
            _mobileSenders = mobileSenders;
            _emailSender = emailSender;
            _memoryCache = memoryCache;
            _jwtKeys = jwtKeys;
        }

        public async Task<UserProfileModel> UserProfile(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException("user not found");
            }

            var userProfile = new UserProfileModel
            {
                FirstName = user.FirstName,
                BirthDate = user.BirthDate,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage,
                Username = user.UserName
            };

            return userProfile;
        }

        public async Task<Result> CreateUserAsync(RegisterModel model)
        {
            try
            {
                bool validEmail = IsValidEmail(model.Confirmation);

                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.Firstname,
                    LastName = model.Lastname,
                    BirthDate = DateTime.Now
                };

                if (!string.IsNullOrWhiteSpace(model.PhoneNumber)) user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded) return result.ToApplicationResult();

                var userDetail = new UserDetail { UserId = user.Id, Username = user.UserName, Fullname = user.FullName() };

                _context.UserDetails.Add(userDetail);

                //await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                return SendConfirmation(validEmail, token, user).Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        private async Task<(Result result, string token)> OTPConfirmation(int OTPNumber, string email)
        {
            var otp = await _context.OTP
                .FirstOrDefaultAsync(e => e.RandomNumber == OTPNumber &&
                e.UserId == email && DateTime.Compare(DateTime.UtcNow, e.ExpiryDate) < 0 && e.Used == false);

            if (otp == null) return (APIResults.InvalidTokenResult("OTP is expired", "کد تایید نادرست است"), null);

            otp.Used = true;

            _context.OTP.Update(otp);

            await _context.SaveChangesAsync();

            return (Result.Success(), otp.Token);
        }

        public async Task<AuthenticationResult> AccountOTPConfirmation(OTPConfirmationModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "User not found", FaError = "حساب کاربری با این نام وجود ندارد" };
            }

            var OTPResult = await OTPConfirmation(model.RandomNumber, model.Email);

            if (!OTPResult.result.Succeeded)
                return new AuthenticationResult { Succeeded = false, EnError = "confirmation code is not valid", FaError = "کد تایید صحیح نمی باشد " };

            var result = await _userManager.ConfirmEmailAsync(user, OTPResult.token);

            await _signInManager.SignInAsync(user, false);

            return await GenerateToken(user);
        }

        public async Task<Result> EmailConfirmation(EmailConfirmationModel model)
        {
            if (model.UserId == null || model.Token == null) return APIResults.BadRequestResult("userId or token is null", "شناسه کاربری یا توکن نادرست است");

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) return APIResults.UserNotFoundResult();

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            return result.ToApplicationResult();
        }

        public async Task<Result> ForgotPassword(SendOtpTokenModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return APIResults.UserNotFoundResult();

            if (!await _userManager.IsEmailConfirmedAsync(user)) return APIResults.NotConfirmedResult("Email not confirmed", "اکانت شما تایید نشده است");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return await SendConfirmation(IsValidEmail(model.Confirmation), token, user);
        }

        public async Task<Result> ForgotPasswordOTPConfirmation(OTPConfirmationModel model)
        {
            var OTPResult = await OTPConfirmation(model.RandomNumber, model.Email);

            string cacheKey = model.Email + "ForgotPasswordOTPConfirmation";

            if (!OTPResult.result.Succeeded) return OTPResult.result;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(cacheKey, OTPResult.token, cacheEntryOptions);

            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null) return APIResults.UserNotFoundResult();

            string cacheKey = model.Email + "ForgotPasswordOTPConfirmation";

            if (_memoryCache.TryGetValue(cacheKey, out string token))
            {
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                return result.ToApplicationResult();
            }

            return APIResults.InvalidTokenResult("Token is expired", "توکن شما انقضا شده است");
        }

        public async Task<Result> SendAccountConfirmationTokenAgain(SendOtpTokenModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return APIResults.UserNotFoundResult();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return SendConfirmation(IsValidEmail(model.Confirmation), token, user).Result;
        }

        private async Task<Result> SendConfirmation(bool validEmail, string token, ApplicationUser user)
        {
            if (validEmail)
            {
                return await _emailSender.SendEmailAsync(user.Email, " ", "Confirmation");
            }
            else
            {
                return await _mobileSenders.SendOTP(user.PhoneNumber, user.Email, token);
            }
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<AuthenticationResult> Login(LoginModel model)
        {
            var user = new ApplicationUser();

            if (IsValidEmail(model.Username))
            {
                user = await _userManager.FindByEmailAsync(model.Username);
            }

            else user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "User not found", FaError = "حساب کاربری با این نام وجود ندارد" };
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

            if (user != null && result.Succeeded)
            {
                var auth = await GenerateToken(user);

                return auth;
            }

            if (result.IsNotAllowed)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "account not confirmed", FaError = "اکانت شما فعال نشده است" };
            }

            return new AuthenticationResult { Succeeded = false, EnError = "username or password is wrong", FaError = "نام کاربری یا رمز عبور نادرست است" };

        }

        public async Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenModel refreshToken)
        {
            var validateToken = GetPrincipalFromToken(refreshToken.Token);

            if (validateToken == null)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "Invalid token", FaError = "توکن نامعتبر است" };
            }

            var expiryDateUnix = long.Parse(validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "This token hasn't expired yet", FaError = "توکن انقضا نشده است" };
            }

            var jti = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storeRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken.RefreshToken);

            if (storeRefreshToken == null)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "This refresh token does not exist", FaError = "توکن بازیابی وجود ندارد" };
            }

            if (storeRefreshToken.Used)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "Token is used ", FaError = "توکن استفاده شده است" };
            }

            if (DateTime.UtcNow > storeRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "This refresh token has expired", FaError = "توکن بازیابی انقضا شده است" };
            }

            if (storeRefreshToken.InValidation)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "This refresh token has been invalidated", FaError = "توکن بازیابی نا معتبر است" };
            }

            if (storeRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Succeeded = false, EnError = "This refresh token does not match this JWT", FaError = "توکن بازیابی نادرست است" };
            }

            storeRefreshToken.Used = true;

            _context.RefreshTokens.Update(storeRefreshToken);

            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validateToken.Claims.Single(x => x.Type == "id").Value);

            return await GenerateToken(user, storeRefreshToken.ExpiryDate);
        }

        public async Task<AuthenticationResult> GenerateToken(ApplicationUser user, DateTime date = default)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtKeys.Value.SigningKey);
            var audience = _jwtKeys.Value.Site;
            int expiryInMinutes = Convert.ToInt32(_jwtKeys.Value.ExpiryInMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.FullName()),
                new Claim("id", user.Id),
            };


            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);

                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = audience,
                Audience = audience,

                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                //Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = date.Year == 1 ? DateTime.UtcNow.AddMonths(6) : date
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            //refreshToken.Token is auto generated
            return new AuthenticationResult
            {
                Succeeded = true,
                RefreshToken = refreshToken.Token,
                Token = tokenHandler.WriteToken(token),
                Username = user.UserName
            };

        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtKeys.Value.SigningKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidAudience = _jwtKeys.Value.Site,
                ValidIssuer = _jwtKeys.Value.Site,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false
            };

            try
            {
                var principle = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validateToken);

                if (!IsJwtWithValidSecurityAlgorithm(validateToken))
                {
                    return null;
                }

                return principle;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validToken)
        {
            return (validToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
