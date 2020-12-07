using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class UserDetail : AuditableEntity
    {
        public string UserId { get; set; }

        public ICollection<UserGame> UserGames  { get; set; }
    }
}
