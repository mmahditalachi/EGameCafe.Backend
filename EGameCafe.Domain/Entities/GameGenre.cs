using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class GameGenre : AuditableEntity
    {
        public string GameGenreId { get; set; }

        public string GameId { get; set; }
        public Game Game { get; set; }

        public string GenreId{ get; set; }
        public Genre Genre { get; set; }
    }
}
