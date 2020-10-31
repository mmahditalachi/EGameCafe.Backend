using EGameCafe.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.FirstOrDefault().Description, FaError(result.Errors.FirstOrDefault().Code));
        }

        private static string FaError(string errorCode)
        {
            string faError = null;

            switch (errorCode)
            {
                case "DuplicateUserName":
                    faError = "نام کاربری قبلا ثبت شده است";
                    break;
                case "InvalidToken":
                    faError = "توکن شما نامعتبر است";
                    break;
                case "PasswordRequiresLower":
                    faError = "رمزعبور باید شامل حداقل یک حرف باشد";
                    break;
                case "DuplicateEmail":
                    faError = "این ایمبل قبلا ثبت شده است";
                    break;
                default:
                    break;
            }

            return faError ??= "سرور در حال بروزرسانی";
        }
    }
}
