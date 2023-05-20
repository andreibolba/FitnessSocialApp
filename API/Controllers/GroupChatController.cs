using API.Interfaces.Repository;

namespace API.Controllers
{
    public class GroupChatController :BaseAPIController
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IGroupChatMessagesRepostitory _groupChatMessageRepository;

        public GroupChatController(IGroupChatRepository groupChatRepository, IGroupChatMessagesRepostitory groupChatMessageRepository)
        {
            _groupChatRepository = groupChatRepository;
            _groupChatMessageRepository = groupChatMessageRepository;
        }
    }
}
