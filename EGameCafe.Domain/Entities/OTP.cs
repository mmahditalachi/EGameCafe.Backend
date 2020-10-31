using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EGameCafe.Domain.Entities
{
    public class OTP
    {
        public OTP()
        {
            Created = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow.AddMinutes(5);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public int RandomNumber { get; set; }
        public DateTime Created { get; set; }
        public bool Used { get; set; }
    }
}
