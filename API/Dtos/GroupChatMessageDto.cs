using API.Models;

namespace API.Dtos
{
    public class GroupChatMessageDto
    {
        public int GroupChatMessageId { get; set; }

        public int PersonId { get; set; }

        public int GroupChatId { get; set; }

        public string Message { get; set; }

        public int? PictureId { get; set; }

        public PictureDto Picture { get; set; }

        public DateTime SendDate { get; set; }

        public GroupChatDto GroupChat { get; set; }

        public PersonDto Person { get; set; }
    }
}
