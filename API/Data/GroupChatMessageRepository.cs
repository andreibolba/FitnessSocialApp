using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class GroupChatMessageRepository : IGroupChatMessagesRepostitory
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public GroupChatMessageRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void DeleteMessage(int groupChatMessageId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupChatMessageDto> GetAllMessages()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GroupChatMessageDto> GetAllMessagesForAGroup(int groupId)
        {
            throw new NotImplementedException();
        }

        public bool SaveAll()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(GroupChatDto meesage)
        {
            throw new NotImplementedException();
        }
    }
}
