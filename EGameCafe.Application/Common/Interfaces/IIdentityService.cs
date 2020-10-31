using EGameCafe.Application.Common.Models;
using EGameCafe.Application.Models;
using EGameCafe.Application.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);
        Task<Result> CreateUserAsync(RegisterModel model);

        Task<AuthenticationResult> Login(LoginModel model);
        Task<Result> DeleteUserAsync(string userId);
        Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenModel refreshToken);
        Task<Result> ForgotPassword(SendOtpTokenModel model);
        Task<Result> ResetPassword(ResetPasswordModel model);
        Task<Result> SendAccountConfirmationTokenAgain(SendOtpTokenModel model);
        Task<Result> AccountOTPConfirmation(OTPConfirmationModel model);
        Task<Result> EmailConfirmation(EmailConfirmationModel model);
        Task<Result> ForgotPasswordOTPConfirmation(OTPConfirmationModel model);
        Task<Result> RegisterUserInfo(RegisterUserInfoModel userInfo);
    }
}
