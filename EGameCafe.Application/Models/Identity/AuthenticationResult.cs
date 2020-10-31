using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EGameCafe.Application.Models
{
    public class AuthenticationResult 
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Succeeded { get; set; }

        public string EnError { get; set; }
        public string FaError { get; set; }
    }
}
