using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Common.Models
{
    public class Result
    {

        internal Result(bool succeeded, string enError, string faError, string type, int status, string title, string id)
        {
            Succeeded = succeeded;
            EnError = enError;
            FaError = faError;
            Type = type;
            Status = status;
            Title = title;
            Id = id;
        }

        public bool Succeeded { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int? Status { get; set; }
        public string EnError { get; set; }
        public string FaError { get; set; }
        public string Id { get; set; }

        public static Result Success(string id = "success",string type = "https://tools.ietf.org/html/rfc7231#section-6.3.1", int status = 200, string title = "Ok")
        {
            return new Result(true, "", "", type, status, title,id);
        }

        public static Result Failure(string enError, string faError, string id = "Failure", string type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            int status = 400, string title = "Bad request")
        {
            return new Result(false, enError, faError, type, status, title, id);
        }

    }
}
