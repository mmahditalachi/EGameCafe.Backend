using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Models.Identity
{
    public class JWTKeys
    {
        public string Site { get; set; }
        public string SigningKey { get; set; }
        public string ExpiryInMinutes { get; set; }
    }
}
