using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Models
{
        public class Result
        {

            internal Result(bool succeeded, string enError, string faError, string type, int status, string title)
            {
                Succeeded = succeeded;
                EnError = enError;
                FaError = faError;
                Type = type;
                Status = status;
                Title = title;
            }

            public bool Succeeded { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public int? Status { get; set; }
            public string EnError { get; set; }
            public string FaError { get; set; }

            public static Result Success(string type = "https://tools.ietf.org/html/rfc7231#section-6.3.1", int status = 200, string title = "Ok")
            {
                return new Result(true, "", "", type, status, title);
            }

            public static Result Failure(string enError, string faError, string type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                int status = 400, string title = "Bad request")
            {
                return new Result(false, enError, faError, type, status, title);
            }

        }
    }
