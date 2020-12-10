using EGameCafe.Domain.Common;
using System.Collections.Generic;

namespace EGameCafe.Domain.Entities
{
    public class Genre : AuditableEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ICollection<GameGenre> GameGenres { get; set; }
    }
}
