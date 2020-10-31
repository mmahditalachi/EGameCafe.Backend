using System;
using System.Collections.Generic;
using System.Text;

namespace EGameCafe.Application.Models.Identity
{
    public class EmailConfirmationModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
