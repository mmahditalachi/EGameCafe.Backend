using EGameCafe.Domain.Common;
using System.Collections.Generic;


namespace EGameCafe.Domain.Entities
{
    public class Game : AuditableEntity
    {
        public string GameId { get; set; }
        public string GameName { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }
        public ICollection<UserGame> UserGames { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}
