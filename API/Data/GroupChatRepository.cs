using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public GroupChatRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateGroupChat(GroupChatDto model)
        {
            throw new NotImplementedException();
        }

        public void DeleteGroupChat(int groupChatID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupChatDto> GetAllGroupChats()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupChatDto> GetAllGroupChatsForLeggedPerson(int personId)
        {
            throw new NotImplementedException();
        }

        public bool SaveAll()
        {
            throw new NotImplementedException();
        }

        public void UpdateGroupChat(GroupChatDto model)
        {
            throw new NotImplementedException();
        }

        public void UpdateMembers(IEnumerable<GroupChatPersonDto> memebers)
        {
            throw new NotImplementedException();
        }
    }
}
