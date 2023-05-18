using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMessageRepository
    {
        public IEnumerable<MessageDto> GetAllMessages();
        public IEnumerable<MessageDto> GetMessages(int currentPersonId,int chatPersonId);
        public IEnumerable<MessageDto> GetLastMessages(int currentPersonId);
        public MessageDto GetMessageById(int id);
        public void AddMessage(MessageDto chat);
        public void DeleteMessage(int id);
        public bool SaveAll();
    }
}
