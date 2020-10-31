using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Models.Identity
{
    public class OTPConfirmationModel
    {
        public int RandomNumber { get; set; }
        public string Email { get; set; }
    }
}
