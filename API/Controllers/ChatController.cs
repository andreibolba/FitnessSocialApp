using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ChatController:BaseAPIController
    {
        private readonly IMessageRepository _messageRepository;

        public ChatController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpGet]
        public ActionResult GetAllMessages()
        {
            return Ok(_messageRepository.GetAllMessages());
        }

        [HttpGet("{personId:int}")]
        public ActionResult GetAllMessagesForCurrentPerson(int personId)
        {
            return Ok(_messageRepository.GetLastMessages(personId));
        }

        [HttpPost("add")]
        public ActionResult AddMessage([FromBody] MessageDto message)
        {
            var res = _messageRepository.AddMessage(message);

            return _messageRepository.SaveAll() ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{meesageId:int}")]
        public ActionResult DeleteMessage(int messageId)
        {
            _messageRepository.DeleteMessage(messageId);

            return _messageRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet("messages/{currentPeronId}/{chatPersonId}")]
        public ActionResult GetChatFromPerson(int currentPeronId, int chatPersonId)
        {
            return Ok(_messageRepository.GetMessages(currentPeronId, chatPersonId));
        }
    }
}
