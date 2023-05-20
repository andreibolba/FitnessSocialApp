using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IGroupChatRepository
    {
        public IEnumerable<GroupChatDto> GetAllGroupChats(); 
        public IEnumerable<GroupChatDto> GetAllGroupChatsForLeggedPerson(int personId);
        public void CreateGroupChat(GroupChatDto model);
        public void UpdateGroupChat(GroupChatDto model);
        public void DeleteGroupChat(int groupChatID);
        public void UpdateMembers(IEnumerable<GroupChatPersonDto> memebers);
        public bool SaveAll();
    }
}
