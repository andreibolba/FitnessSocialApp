using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddMessage(MessageDto chat)
        {
            var message = _mapper.Map<Chat>(chat);
            _context.Chats.Add(message);
        }

        public void DeleteMessage(int id)
        {
            var message = GetMessageById(id);
            message.Deleted = true;
            _context.Chats.Update(_mapper.Map<Chat>(message));
        }

        public IEnumerable<MessageDto> GetAllMessages()
        {
            var allMessages = _context.Chats.Where(c => c.Deleted == false);
            return _mapper.Map<IEnumerable<MessageDto>>(allMessages);
        }

        public IEnumerable<MessageDto> GetLastMessages(int currentPersonId)
        {
            var allMessagesCurrentPersonAsSender = GetAllMessages().Where(m => m.PersonSenderId == currentPersonId);
            var allMessagesCurrentPersonAsReceiver = GetAllMessages().Where(m => m.PersonReceiverId == currentPersonId);

            var allLastMessages = new List<MessageDto>();

            foreach(var m in allMessagesCurrentPersonAsSender)
            {
                if (allLastMessages.Where(m => m.PersonReceiverId == m.PersonReceiverId).Count() == 0)
                    allLastMessages.Add(m);
            }

            foreach (var m in allMessagesCurrentPersonAsReceiver)
            {
                if (allLastMessages.Where(m => m.PersonSenderId == m.PersonSenderId).Count() == 0)
                    allLastMessages.Add(m);
            }

            return allLastMessages;
        }

        public MessageDto GetMessageById(int id)
        {
            return GetAllMessages().SingleOrDefault(m => m.ChatId == id);
        }

        public IEnumerable<MessageDto> GetMessages(int currentPersonId, int chatPersonId)
        {
            return GetAllMessages().Where(m => (m.PersonReceiverId == currentPersonId && m.PersonSenderId == chatPersonId)
            || (m.PersonReceiverId == chatPersonId && m.PersonSenderId == currentPersonId)).OrderBy(m => m.SendDate);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
