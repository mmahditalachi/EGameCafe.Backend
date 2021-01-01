using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Domain.Entities
{
    public class UserFriend
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public UserDetail User { get; set; }

        public string FriendId { get; set; }

        public UserDetail Friend { get; set; }
    }
}
