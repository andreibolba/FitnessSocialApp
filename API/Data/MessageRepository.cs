using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IPictureRepository _pictureRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IMapper _mapper;

        public MessageRepository(InternShipAppSystemContext context, IPictureRepository pictureRepo, IPersonRepository personRepo, IMapper mapper)
        {
            _context = context;
            _pictureRepo = pictureRepo;
            _personRepo = personRepo;
            _mapper = mapper;
        }

        public MessageDto AddMessage(MessageDto chat)
        {
            var message = _mapper.Map<Chat>(chat);
            message.SendDate = DateTime.Now;
            _context.Chats.Add(message);
            return SaveAll() ? _mapper.Map<MessageDto>(message) : null;
        }

        public void DeleteChat(int personId, int chatPersonId)
        {
            var chat = _context.Chats.Where(r => (r.PersonSenderId == personId && r.PersonReceiverId == chatPersonId) || (r.PersonSenderId == chatPersonId && r.PersonReceiverId == personId)).ToList();
            foreach(var c in chat)
            {
                c.Deleted = true;
                _context.Chats.Update(c);
            }
        }

        public void DeleteMessage(int id)
        {
            var message = _mapper.Map<Chat>(GetMessageById(id));
            message.Deleted = true;
            _context.Chats.Update(message);
        }

        public IEnumerable<MessageDto> GetAllMessages()
        {
            var allMessages = _context.Chats.Where(c => c.Deleted == false && c.PersonReceiver.Deleted==false).Include(m=>m.PersonReceiver).Include(m=>m.PersonSender);
            var allMessagesDTO = _mapper.Map<IEnumerable<MessageDto>>(allMessages);
            foreach (var message in allMessagesDTO)
            {
                message.PersonReceiver.Picture = message.PersonReceiver.PictureId ==null? null : _pictureRepo.GetById(message.PersonReceiver.PictureId.Value);
                message.PersonSender.Picture = message.PersonSender.PictureId==null? null : _pictureRepo.GetById(message.PersonSender.PictureId.Value);
            }
            return allMessagesDTO;
        }

        public IEnumerable<MessageDto> GetLastMessages(int currentPersonId)
        {
            var allMessageForCurentPerson = GetAllMessages().Where(m => m.PersonSenderId == currentPersonId || m.PersonReceiverId == currentPersonId).OrderByDescending(m => m.SendDate);

            var allLastMessages = new List<MessageDto>();

            foreach(var m in allMessageForCurentPerson)
            {
                int findId = m.PersonReceiverId == currentPersonId ? m.PersonSenderId : m.PersonReceiverId;

                if (allLastMessages.Where(mes => mes.PersonReceiverId == findId || mes.PersonSenderId == findId).Count() == 0)
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

        public IEnumerable<MessageDto> GetMessagesByUseranem(string currentPersonUsername, string chatPersonUsername)
        {
            var s = _personRepo.GetPersonByUsername(currentPersonUsername).PersonId;
            var r = _personRepo.GetPersonByUsername(chatPersonUsername).PersonId;
            var a = 0;
            return GetMessages(_personRepo.GetPersonByUsername(currentPersonUsername).PersonId, _personRepo.GetPersonByUsername(chatPersonUsername).PersonId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync()>0;
        }
    }
}
