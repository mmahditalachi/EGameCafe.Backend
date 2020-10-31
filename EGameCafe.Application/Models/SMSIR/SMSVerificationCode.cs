using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Models.SMSIR
{
    public class SMSVerificationCode
    {
        public double VerificationCodeId { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
