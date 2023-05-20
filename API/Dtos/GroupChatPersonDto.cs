namespace API.Dtos
{
    public class GroupChatPersonDto
    {
        public int GroupChatUserId { get; set; }

        public int PersonId { get; set; }

        public int GroupChatId { get; set; }

        public GroupChatDto GroupChat { get; set; }

        public PersonDto Person { get; set; }
    }
}
