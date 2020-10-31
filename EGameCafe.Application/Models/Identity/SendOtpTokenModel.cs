using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Models.Identity
{
    public class SendOtpTokenModel
    {
        public string Email { get; set; }
        public string Confirmation { get; set; }
    }
}
