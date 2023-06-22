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
        private readonly IPersonRepository _personRepository;
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public GroupChatRepository(InternShipAppSystemContext context, IPersonRepository personRepository, IPictureRepository pictureRepository, IMapper mapper)
        {
            _context = context;
            _personRepository = personRepository;
            _pictureRepository = pictureRepository;
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
            var group = _mapper.Map<GroupChat>(_context.GroupChats.SingleOrDefault(g => g.GroupChatId == groupChatID));
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

        public void DeleteMember(int personId, int groupChatId)
        {
            var res = _context.GroupChatPeople.FirstOrDefault(p=>p.PersonId == personId && p.GroupChatId == groupChatId);
            res.Deleted = true;
            _context.GroupChatPeople.Update(res);
        }

        public IEnumerable<GroupChatDto> GetAllGroupChats()
        {
            var allGroupChats = _mapper.Map<IEnumerable<GroupChatDto>>(_context.GroupChats.Where(g => g.Deleted == false).Include(g=>g.GroupChatMessages));
            foreach(var g in allGroupChats)
            {
                var allParticipants = _context.GroupChatPeople.Where(gcp => gcp.Deleted == false && gcp.GroupChatId == g.GroupChatId).Include(gcp => gcp.Person).Select(gcp => gcp.Person);
                var pas = _mapper.Map<IEnumerable<PersonDto>>(allParticipants);
                foreach(var person in pas)
                {
                    person.Picture = person.PictureId == null ? null : _pictureRepository.GetById(person.PictureId.Value);
                }
                g.Participants = pas;
                g.Picture = g.PictureId == null ? null : _pictureRepository.GetById(g.PictureId.Value);
            }
            return allGroupChats;
        }

        public IEnumerable<GroupChatDto> GetAllGroupChatsForAPerson(int personId)
        {
            var group = GetAllGroupChats().Where(g => g.AdminId == personId).ToList();
            var groupThatIAmNotAdmin = _context.GroupChatMessages.Where(g=>g.PersonId == personId && g.Deleted==false).ToList();

            foreach(var o in groupThatIAmNotAdmin)
            {
                if (group.Where(g => g.GroupChatId == o.GroupChatId).Count() == 0)
                {
                    var groupT = GetGroupChatById(o.GroupChatId);
                    if(groupT != null)
                    group.Add(groupT);
                }
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
                newest.Person = _personRepository.GetPersonById(newest.PersonId);
                lastMessages.Add(newest);
            }
            return lastMessages;
        }

        public GroupChatDto GetGroupChatById(int groupChatId)
        {
            return GetAllGroupChats().SingleOrDefault(g => g.GroupChatId == groupChatId);
        }

        public void MakeAdmin(int personId, int groupChatId)
        {
            var res = _context.GroupChats.FirstOrDefault(p => p.GroupChatId == groupChatId);
            var id = res.AdminId;
            res.AdminId = personId;
            _context.GroupChats.Update(res);
            var p = _context.GroupChatPeople.FirstOrDefault(g => g.GroupChatId == groupChatId && g.PersonId == personId);
            if(p != null)
            {
                p.Deleted = false;
                _context.GroupChatPeople.Update(p);
            }else
            _context.GroupChatPeople.Add(new GroupChatPerson()
            {
                PersonId = id,
                GroupChatId = groupChatId,
                Deleted = false
            });
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public GroupChatDto UpdateGroupChat(GroupChatDto model)
        {
            var groupChat = _mapper.Map<GroupChat>(GetGroupChatById(model.GroupChatId));

            groupChat.GroupChatName = model.GroupChatName == null ? groupChat.GroupChatName : model.GroupChatName;
            groupChat.GroupChatDescription = model.GroupChatDescription;
            groupChat.AdminId = model.AdminId == null ? groupChat.AdminId : model.AdminId.Value;
            groupChat.PictureId = model.PictureId == null ? groupChat.PictureId : model.PictureId.Value;

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

        public bool UpdateMembers(int groupChatId, List<int> members)
        {
            var result = _context.GroupChatPeople.Where(g => g.Deleted == false && g.GroupChatId == groupChatId);
            if (result.Count() == 0 && members.Count() == 0)
                return false;
            if (members.Count() == 0)
            {
                foreach (var res in result)
                {
                    res.Deleted = true;
                    _context.GroupChatPeople.Update(res);
                }

                return true;
            }  

            foreach (var res in result)
            {
                bool delete = members.IndexOf(res.PersonId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    members.Remove(res.PersonId);
                _context.GroupChatPeople.Update(res);
            }

            foreach (var id in members)
            {
                _context.GroupChatPeople.Add(new GroupChatPerson
                {
                    GroupChatId = groupChatId,
                    PersonId = id,
                    Deleted = false
                });
            }
            return true;
        }
    }
}
