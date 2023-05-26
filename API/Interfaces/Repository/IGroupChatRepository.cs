using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IGroupChatRepository
    {
        public IEnumerable<GroupChatDto> GetAllGroupChats(); 
        public GroupChatDto GetGroupChatById(int groupChatId);
        public IEnumerable<GroupChatMessageDto> GetAllLastMessagesForAPerson(int personId);
        public IEnumerable<GroupChatDto> GetAllGroupChatsForAPerson(int personId);
        public GroupChatDto CreateGroupChat(GroupChatDto model, List<int> groupMembers);
        public GroupChatDto UpdateGroupChat(GroupChatDto model);
        public void DeleteGroupChat(int groupChatID);
        public bool UpdateMembers(List<int> memebers, int groupChatId);
        public bool SaveAll();
    }
}
