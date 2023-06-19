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

        public int? PictureId { get; set; }

        public PictureDto Picture { get; set; }

        public PersonDto PersonReceiver { get; set; }

        public PersonDto PersonSender { get; set; }
    }
}
