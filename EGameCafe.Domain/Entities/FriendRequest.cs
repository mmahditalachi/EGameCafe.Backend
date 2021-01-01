using EGameCafe.Domain.Enums;


namespace EGameCafe.Domain.Entities
{
    public class FriendRequest
    {
        public string Id { get; set; }

        public string SenderId { get; set; }

        public UserDetail Sender { get; set; }

        public string ReceiverId { get; set; }

        public UserDetail Receiver { get; set; }

        public FriendRequestStatus FriendRequestStatus { get; set; }
    }
}
