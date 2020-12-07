using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class UserGame : AuditableEntity
    {
        public string UserGameId { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }

        public string UserId { get; set; }
        public UserDetail UserDetail { get; set; }
    }
}
