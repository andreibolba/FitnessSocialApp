using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace API.Controllers
{
    public class GroupChatFromClient
    {
        public string NameOfGroup { get; set; }
        public string DescriptionOfGroup { get; set; }
        public int AdminId { get; set; }
        public List<int> Ids { get; set; }
    }

    public class GroupChatController : BaseAPIController
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IGroupChatMessagesRepostitory _groupChatMessageRepository;

        public GroupChatController(IGroupChatRepository groupChatRepository, IGroupChatMessagesRepostitory groupChatMessageRepository)
        {
            _groupChatRepository = groupChatRepository;
            _groupChatMessageRepository = groupChatMessageRepository;
        }

        [HttpGet]
        public ActionResult GetAllGroupChats()
        {
            return Ok(_groupChatRepository.GetAllGroupChats());
        }

        [HttpGet("{personId:int}")]
        public ActionResult GetAllGroupChatsForPerson(int personId)
        {
            var res = _groupChatRepository.GetAllLastMessagesForAPerson(personId);
            return Ok(res);
        }

        [HttpGet("messages/{groupId:int}")]
        public ActionResult GetAllMessagesForGroup(int groupId)
        {
            return Ok(_groupChatMessageRepository.GetAllMessagesForAGroup(groupId));
        }

        [HttpPost("add")]
        public ActionResult AddGroupChat([FromBody] object groupChat)
        {

            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(groupChat.ToString())))
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(GroupChatFromClient));
                GroupChatFromClient groupChatModel = (GroupChatFromClient)deserializer.ReadObject(ms);
                GroupChatDto groupChatDto = new GroupChatDto()
                {
                    AdminId = groupChatModel.AdminId,
                    GroupChatName = groupChatModel.NameOfGroup,
                    GroupChatDescription = groupChatModel.DescriptionOfGroup
                };
                var res = _groupChatRepository.CreateGroupChat(groupChatDto, groupChatModel.Ids);
                var message = _groupChatMessageRepository.GetAllMessagesForAGroup(res.GroupChatId).Last();
                return res!=null? Ok(message):BadRequest("InternalServerError");
            }
        }

        [HttpPost("message/add")]
        public ActionResult AddMessageToGroup([FromBody] GroupChatMessageDto messageDto)
        {
            var res = _groupChatMessageRepository.SendMessage(messageDto);

            return res != null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("message/delete")]
        public ActionResult DeleteMessageToGroup([FromBody] GroupChatMessageDto messageDto)
        {
            _groupChatMessageRepository.SendMessage(messageDto);

            return _groupChatMessageRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

    }
}
