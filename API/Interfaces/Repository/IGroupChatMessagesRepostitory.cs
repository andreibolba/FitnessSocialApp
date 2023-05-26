using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IGroupChatMessagesRepostitory
    {
        public IEnumerable<GroupChatMessageDto> GetAllMessages();
        public IEnumerable<GroupChatMessageDto> GetAllMessagesForAGroup(int groupChatId);
        public GroupChatMessageDto SendMessage(GroupChatMessageDto message);
        public GroupChatMessageDto GetMessageById(int groupChatMessageId);
        public void DeleteMessage(int groupChatMessageId);
        public bool SaveAll();
    }
}
