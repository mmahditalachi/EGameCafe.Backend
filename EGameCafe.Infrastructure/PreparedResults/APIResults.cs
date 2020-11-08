using EGameCafe.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Infrastructure.PreparedResults
{
    public static class APIResults
    {
        public static Result BadRequestResult(string enErrorMessage, string faErrorMessage)
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.5.1", 400, "Bad Request");
        }

        public static Result NotFoundResult(string enErrorMessage, string faErrorMessage)
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.5.4", 404, "Not found");
        }

        public static Result InternalServerResult(string enErrorMessage, string faErrorMessage)
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.6.1", 500, "Internal Server Error");
        }

        public static Result InvalidTokenResult(string enErrorMessage, string faErrorMessage)
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.6.1", 498, "Invalid Token");
        }

        public static Result UserNotFoundResult(string enErrorMessage = "User not found", string faErrorMessage = "کاربری با این شناسه یافت نشد")
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.5.4", 404, "Not found");
        }

        public static Result NotConfirmedResult(string enErrorMessage, string faErrorMessage)
        {
            return Result.Failure("Failure", enErrorMessage, faErrorMessage, "https://tools.ietf.org/html/rfc7231#section-6.5.4", 401, "Unauthorized");
        }
    }
}
