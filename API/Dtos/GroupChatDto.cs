using API.Models;

namespace API.Dtos
{
    public class GroupChatDto
    {
        public int GroupChatId { get; set; }

        public string GroupChatName { get; set; }

        public int AdminId { get; set; }

        public PersonDto Admin { get; set; }
    }
}
