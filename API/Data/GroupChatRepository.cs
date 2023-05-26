using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace API.Data
{
    public class GroupChatRepository : IGroupChatRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IPersonRepository _person;
        private readonly IMapper _mapper;

        public GroupChatRepository(InternShipAppSystemContext context, IPersonRepository person, IMapper mapper)
        {
            _context = context;
            _person = person;
            _mapper = mapper;
        }

        public GroupChatDto CreateGroupChat(GroupChatDto model, List<int> groupMembers)
        {
            var modelToDb = _mapper.Map<GroupChat>(model);
            _context.GroupChats.Add(modelToDb);
            if (SaveAll() == false)
                return null;
            foreach(var member in groupMembers)
            {
                var memberOfGroup = new GroupChatPersonDto()
                {
                    GroupChatId = modelToDb.GroupChatId,
                    PersonId = member
                };
                _context.GroupChatPeople.Add(_mapper.Map<GroupChatPerson>(memberOfGroup));
                if (SaveAll() == false)
                    return null;
            }
            var message = new GroupChatMessage()
            {
                GroupChatId = modelToDb.GroupChatId,
                PersonId = modelToDb.AdminId,
                Message = "Hello I have created this group!",
                SendDate = DateTime.Now
            };
            _context.GroupChatMessages.Add(message);
            return SaveAll()? _mapper.Map<GroupChatDto>(modelToDb):null;
        }

        public void DeleteGroupChat(int groupChatID)
        {
            var group = _mapper.Map<GroupChat>(GetGroupChatById(groupChatID));
            group.Deleted = true;
            _context.GroupChats.Update(group);
            foreach(var g in _context.GroupChatPeople.Where(p=>p.GroupChatId == group.GroupChatId))
            {
                g.Deleted = true;
                _context.GroupChatPeople.Update(g);
            }

            foreach (var g in _context.GroupChatMessages.Where(p => p.GroupChatId == group.GroupChatId))
            {
                g.Deleted = true;
                _context.GroupChatMessages.Update(g);
            }
        }

        public IEnumerable<GroupChatDto> GetAllGroupChats()
        {
            var allGroupChats = _mapper.Map<IEnumerable<GroupChatDto>>(_context.GroupChats.Where(g => g.Deleted == false).Include(g=>g.GroupChatMessages));
            foreach(var g in allGroupChats)
            {
                var allParticipants = _context.GroupChatPeople.Where(gcp => gcp.Deleted == false && gcp.GroupChatId == g.GroupChatId).Include(gcp => gcp.Person).Select(gcp => gcp.Person);
                g.Participants = _mapper.Map<IEnumerable<PersonDto>>(allParticipants);
            }
            return allGroupChats;
        }

        public IEnumerable<GroupChatDto> GetAllGroupChatsForAPerson(int personId)
        {
            var group = GetAllGroupChats().Where(g => g.AdminId == personId).ToList();
            var other = _context.GroupChatPeople.Where(g => g.PersonId == personId && g.Deleted == false).Include(g => g.GroupChat).Select(g => g.GroupChat);
            foreach(var o in other)
            {
                group.Add(_mapper.Map<GroupChatDto>(o)); 
            }
            return group;
        }

        public IEnumerable<GroupChatMessageDto> GetAllLastMessagesForAPerson(int personId)
        {
            var group = GetAllGroupChatsForAPerson(personId);
            var lastMessages = new List<GroupChatMessageDto>();
            foreach(var g in group)
            {
                var orderedMessages = g.GroupChatMessages.OrderByDescending(g => g.SendDate).ToList();
                var orderedMessages2 = g.GroupChatMessages.OrderBy(g => g.SendDate).ToList();
                var newest = orderedMessages.First();
                newest.Person = _person.GetPersonById(newest.PersonId);
                lastMessages.Add(newest);
            }
            return lastMessages;
        }

        public GroupChatDto GetGroupChatById(int groupChatId)
        {
            return GetAllGroupChats().SingleOrDefault(g => g.GroupChatId == groupChatId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public GroupChatDto UpdateGroupChat(GroupChatDto model)
        {
            var groupChat = _mapper.Map<GroupChat>(GetGroupChatById(model.GroupChatId));

            groupChat.GroupChatName = model.GroupChatName == null ? groupChat.GroupChatName : model.GroupChatName;
            groupChat.AdminId = model.AdminId == null ? groupChat.AdminId : model.AdminId.Value;

            _context.GroupChats.Update(groupChat);

            return SaveAll() ? _mapper.Map<GroupChatDto>(groupChat) : null;
        }

        public bool UpdateMembers(List<int> memebers, int groupChatId)
        {
            var result = _context.GroupChatPeople.Where(g => g.Deleted == false && g.GroupChatId == groupChatId);
            if (result.Count() == 0 && memebers.Count() == 0)
                return false;
            foreach (var res in result)
            {
                bool delete = memebers.IndexOf(res.PersonId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    memebers.Remove(res.PersonId);
                _context.GroupChatPeople.Update(res);
            }

            foreach (var id in memebers)
            {
                _context.GroupChatPeople.Add(new GroupChatPerson
                {
                    PersonId = id,
                    GroupChatId = groupChatId,
                    Deleted = false
                });
            }
            return true;
        }
    }
}
