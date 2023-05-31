using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMessageRepository
    {
        public IEnumerable<MessageDto> GetAllMessages();
        public IEnumerable<MessageDto> GetMessages(int currentPersonId,int chatPersonId);
        public IEnumerable<MessageDto> GetLastMessages(int currentPersonId);
        public MessageDto GetMessageById(int id);
        public MessageDto AddMessage(MessageDto chat);
        public void DeleteMessage(int id);
        public void DeleteChat(int personId,int chatPersonId);
        public bool SaveAll();
    }
}
