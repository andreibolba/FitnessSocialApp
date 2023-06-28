using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IGroupChatRepository
    {
        public IEnumerable<GroupChatDto> GetAllGroupChats(); 
        public GroupChatDto GetGroupChatById(int groupChatId);
        public Task<GroupChatDto> GetGroupChatByIdAsync(int groupChatId);
        public IEnumerable<GroupChatMessageDto> GetAllLastMessagesForAPerson(int personId);
        public IEnumerable<GroupChatDto> GetAllGroupChatsForAPerson(int personId);
        public GroupChatDto CreateGroupChat(GroupChatDto model, List<int> groupMembers);
        public GroupChatDto UpdateGroupChat(GroupChatDto model);
        public void DeleteGroupChat(int groupChatID);
        public bool UpdateMembers(List<int> memebers, int groupChatId);
        public void DeleteMember(int personId, int groupChatId);
        public void MakeAdmin(int personId, int groupChatId);
        public bool UpdateMembers(int groupChatId, List<int> members);
        public bool SaveAll();
    }
}
