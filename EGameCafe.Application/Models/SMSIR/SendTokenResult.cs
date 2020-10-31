using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Models.SMSIR
{
    public class SendTokenResult
    {
        public string TokenKey { get; set; }
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
    }
}
