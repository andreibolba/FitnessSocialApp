using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IGroupChatMessagesRepostitory
    {
        public IEnumerable<GroupChatMessageDto> GetAllMessages();
        public IEnumerable<GroupChatMessageDto> GetAllMessagesForAGroup(int groupId);
        public void SendMessage(GroupChatDto meesage);
        public void DeleteMessage(int groupChatMessageId);
        public bool SaveAll();
    }
}
