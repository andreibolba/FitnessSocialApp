using API.Data;
using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Task = System.Threading.Tasks.Task;

namespace API.SignalR
{
    public class GroupMessageHub : Hub
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IGroupChatMessagesRepostitory _groupChatMessagesRepostitory;
        private readonly IPersonRepository _personRepository;
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IMapper _mapper;

        public GroupMessageHub(InternShipAppSystemContext context, IGroupChatMessagesRepostitory groupChatMessagesRepostitory, IPersonRepository personRepository, IGroupChatRepository groupChatRepository, IMapper mapper)
        {
            _context = context;
            _groupChatMessagesRepostitory = groupChatMessagesRepostitory;
            _personRepository = personRepository;
            _groupChatRepository = groupChatRepository;
            _mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            int groupId = -1;
            var parse = int.TryParse(httpContext.Request.Query["group"].ToString(), out groupId);
            var groupName = $"groupNumber:{groupId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var messages = _groupChatMessagesRepostitory.GetAllMessagesForAGroup(groupId);

            await Clients.Group(groupName).SendAsync("ReceiveGroupMessageThread", messages);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        public async Task AddGroupChatMessage(GroupChatMessageDto message)
        {
            var messageToDb = _mapper.Map<GroupChatMessage>(message);
            messageToDb.SendDate = DateTime.Now;
            _context.GroupChatMessages.Add(messageToDb);
            if (await _groupChatMessagesRepostitory.SaveAllAsync())
            {
                var mess = _mapper.Map<GroupChatMessageDto>(messageToDb);
                mess.Person = await _personRepository.GetPersonByIdAsync(messageToDb.PersonId);
                mess.GroupChat = await _groupChatRepository.GetGroupChatByIdAsync(messageToDb.GroupChatId);
                await Clients.Group($"groupNumber:{message.GroupChatId}").SendAsync("NewGroupChatMessage", mess);
            }
        }
    }
}
