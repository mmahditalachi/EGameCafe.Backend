using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGameCafe.Application.Models.FriendRequest
{
    public class GetAllFriendRequestDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
    }
}
