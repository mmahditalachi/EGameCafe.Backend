using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Models.Identity
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
