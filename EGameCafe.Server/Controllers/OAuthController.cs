using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EGameCafe.Application.Common.Interfaces;
using EGameCafe.Application.Models;
using EGameCafe.Application.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EGameCafe.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public OAuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _identityService.CreateUserAsync(model);
            return result.Succeeded
                ? (IActionResult)CreatedAtAction(nameof(GetUser), result)
                : BadRequest(result);
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var result = await _identityService.GetUserNameAsync(userId);
            return result != null ? (IActionResult)Ok(result) : BadRequest(new { error = "user not found" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _identityService.Login(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel model)
        {
            var result = await _identityService.RefreshTokenAsync(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(SendOtpTokenModel model)
        {
            var result = await _identityService.ForgotPassword(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("ForgotPasswordOTPConfirmation")]
        public async Task<IActionResult> ForgotPasswordOTPConfirmation(OTPConfirmationModel model)
        {
            var result = await _identityService.ForgotPasswordOTPConfirmation(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var result = await _identityService.ResetPassword(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("SendAccountConfirmationTokenAgain")]
        public async Task<IActionResult> SendConfirmTokenAgain(SendOtpTokenModel model)
        {
            var result = await _identityService.SendAccountConfirmationTokenAgain(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(EmailConfirmationModel model)
        {
            var result = await _identityService.EmailConfirmation(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("AccountOTPConfirmation")]
        public async Task<IActionResult> AccountOTPConfirmation(OTPConfirmationModel model)
        {
            var result = await _identityService.AccountOTPConfirmation(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }

        [HttpPost("RegisterUserInfo")]
        public async Task<IActionResult> RegisterUserInfo(RegisterUserInfoModel model)
        {
            var result = await _identityService.RegisterUserInfo(model);
            return result.Succeeded ? (IActionResult)Ok(result) : BadRequest(result);
        }
    }
}
