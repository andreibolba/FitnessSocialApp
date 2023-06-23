using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;

        public MessageHub(IMessageRepository messageRepository, InternShipAppSystemContext context, IMapper mapper, IPersonRepository personRepository)
        {
            _messageRepository = messageRepository;
            _context = context;
            _mapper = mapper;
            _personRepository = personRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.UserIdentifier, otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            
            var messages = _messageRepository.GetMessagesByUseranem(Context.UserIdentifier, otherUser);

            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task AddMessage(MessageDto chat)
        {
            var message = _mapper.Map<Chat>(chat);
            var sender = await _personRepository.GetPersonByIdAsync(message.PersonSenderId);
            var receiver = await _personRepository.GetPersonByIdAsync(message.PersonReceiverId);
            message.SendDate = DateTime.Now;
            _context.Chats.Add(message);

            if(await _messageRepository.SaveAllAsync())
            {
                var group = GetGroupName(sender.Username, receiver.Username);
                var mesageToSendToClient = _mapper.Map<MessageDto>(message);
                mesageToSendToClient.PersonSender = sender;
                mesageToSendToClient.PersonReceiver = receiver;
                await Clients.Group(group).SendAsync("NewMessage", mesageToSendToClient);
            }
        }

        private string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other)<0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
