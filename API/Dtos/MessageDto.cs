using API.Models;

namespace API.Dtos
{
    public class MessageDto
    {
        public int ChatId { get; set; }

        public int PersonSenderId { get; set; }

        public int PersonReceiverId { get; set; }

        public string Message { get; set; }

        public DateTime SendDate { get; set; }

        public bool Deleted { get; set; }

        public virtual Person PersonReceiver { get; set; }

        public virtual Person PersonSender { get; set; }
    }
}
