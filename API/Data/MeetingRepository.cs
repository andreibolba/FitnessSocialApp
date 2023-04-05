using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public sealed class MeetingRepository : IMeetingRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public MeetingRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public MeetingDto Create(MeetingDto meeting)
        {
            var meet = _mapper.Map<Meeting>(meeting);
            _context.Meetings.Add(meet);
            if (SaveAll() == false)
                return null;
            CreateUpdateDelete(meeting.InterndIds, meet.MeetingId, 1);
            CreateUpdateDelete(meeting.GroupIds, meet.MeetingId, 2);
            _context.SaveChanges();
            return _mapper.Map<MeetingDto>(meet);
        }

        public void Delete(int id)
        {
            var meet = _mapper.Map<Meeting>(GetMeetingById(id));
            meet.Deleted = true;
            var allMeetingsConnection = _context.MeetingInternGroups.Where(mg => mg.MeetingId == id);
            foreach (var meetCon in allMeetingsConnection)
            {
                meetCon.Deleted = true;
                _context.MeetingInternGroups.Update(meetCon);
            }
            _context.Meetings.Update(meet);
        }

        public IEnumerable<MeetingDto> GetAll()
        {
            var result = _mapper.Map<IEnumerable<MeetingDto>>(_context.Meetings.Where(m => m.Deleted == false).Include(m => m.Trainer));
            foreach (var r in result)
            {
                var allPeopleInMeeting = new List<PersonDto>();
                var allPeople = _context.GetPeopleInGroupMeetings.Where(mg => mg.MeetingId == r.MeetingId);
                foreach (var p in allPeople)
                    allPeopleInMeeting.Add(_mapper.Map<PersonDto>(p));

                var allinterns = _context.MeetingInternGroups.Where(mg => mg.Deleted == false && mg.GroupId == null && mg.MeetingId == r.MeetingId).Include(mg => mg.Intern).Select(mg => mg.Intern).AsEnumerable();
                foreach (var intern in allinterns)
                {
                    if (allPeopleInMeeting.FirstOrDefault(all => all.PersonId == intern.PersonId) == null)
                        allPeopleInMeeting.Add(_mapper.Map<PersonDto>(intern));
                }

                allPeopleInMeeting.Add(r.Trainer);
                r.AllPeopleInMeeting = allPeopleInMeeting;
            }
            return result;
        }

        public IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count = null)
        {
            var getAllMeetings = GetFutureMeetings().Where(m => m.TrainerId == trainerId);
            if (count != null && getAllMeetings.Count() >= count.Value)
                return getAllMeetings.Take(count.Value);
            return getAllMeetings;
        }

        public MeetingDto GetMeetingById(int id)
        {
            return GetAll().FirstOrDefault(m => m.MeetingId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(MeetingDto meeting)
        {
            var meet = _mapper.Map<Meeting>(GetMeetingById(meeting.MeetingId));
            meet.MeetingLink = meeting.MeetingLink;
            meet.MeetingName = meeting.MeetingName;
            meet.MeetingStartTime = meeting.MeetingStartTime;
            meet.MeetingFinishTime = meeting.MeetingFinishTime;
            _context.Meetings.Update(meet);
            CreateUpdateDelete(meeting.InterndIds, meeting.MeetingId, 1);
            CreateUpdateDelete(meeting.GroupIds, meeting.MeetingId, 2);
        }

        public IEnumerable<MeetingDto> GetFutureMeetings()
        {
            return GetAll().Where(r => Utils.Utils.IsValidDate(r) == true).OrderBy(m => m.MeetingStartTime).ThenBy(m => m.MeetingFinishTime);
        }

        //op-1=>intern
        //op-2=>group
        public bool CreateUpdateDelete(string obj, int meetingId, int op)
        {
            var result = op == 1
               ? _context.MeetingInternGroups.Where(t => t.Deleted == false && t.MeetingId == meetingId && t.GroupId == null)
               : _context.MeetingInternGroups.Where(t => t.Deleted == false && t.MeetingId == meetingId && t.InternId == null);
            if (obj.Length == 0 || obj == null)
                return false;
            List<int> idList = Utils.Utils.FromStringToInt(obj + "!");
            if (result.Count() == 0 && idList.Count() == 0)
                return false;
            foreach (var res in result)
            {
                int objId = op == 1 ? res.InternId.Value : res.GroupId.Value;
                bool delete = idList.IndexOf(objId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(objId);
                _context.MeetingInternGroups.Update(res);
            }

            foreach (var id in idList)
            {
                _context.MeetingInternGroups.Add(new MeetingInternGroup
                {
                    MeetingId = meetingId,
                    GroupId = op == 2 ? id : null,
                    InternId = op == 1 ? id : null,
                    Deleted = false
                });
            }
            return true;
        }

        //Pentru mine din viitor. Codul asta merge:)) Facem pariu ca te intorci peste cateva saptamani si o sa stii
        //ce dracu ai scris aici? Dar partea buna e ca merge:))))
        public IEnumerable<MeetingDto> GetAllByInternId(int internId, int? count = null)
        {
            var allMeetings=GetAll();
            var allGroupsInMeeting = _context.MeetingInternGroups.Where(mg => mg.GroupId != null && mg.Deleted == false).ToList();
            List<MeetingDto> allMeetingByPerson = new List<MeetingDto>();
            var allPeople = _context.InternGroups.Where(ig => ig.InternId == internId && ig.Deleted == false).ToList();
            foreach (var person in allPeople)
            {
                var allMeetingsWithGroupId = allGroupsInMeeting.Where(gm=>gm.GroupId==person.GroupId);
                foreach (var meet in allMeetingsWithGroupId)
                {
                    if (allMeetingByPerson.FirstOrDefault(m => m.MeetingId == meet.MeetingId) == null)
                        allMeetingByPerson.Add(allMeetings.FirstOrDefault(m=>m.MeetingId==meet.MeetingId));
                }
            }

            var allMeetingWithPerson = _context.MeetingInternGroups.Where(m => m.Deleted == false && m.InternId == internId).Include(m => m.Meeting).Select(m => m.Meeting);
            foreach (var meet in allMeetingWithPerson)
                if (allMeetingByPerson.FirstOrDefault(m => m.MeetingId == meet.MeetingId) == null)
                    allMeetingByPerson.Add(allMeetings.FirstOrDefault(m=>m.MeetingId==meet.MeetingId));

            if (count != null && allMeetingByPerson.Count() >= count.Value)
                return allMeetingByPerson.Take(count.Value);

            return allMeetingByPerson;
        }

        public IEnumerable<MeetingDto> GetAllByGroupId(int groupId, int? count = null)
        {
            var allMeetings= GetFutureMeetings();
            var gettAllGroupsInMeeting = _context.MeetingInternGroups.Where(mg => mg.GroupId == groupId && mg.Deleted == false);
            List<MeetingDto> allMetingsWithGroupId=new List<MeetingDto>();
            foreach (var a in gettAllGroupsInMeeting)
            {
                var meet = allMeetings.FirstOrDefault(m => m.MeetingId == a.MeetingId);
                if (meet!=null)
                    allMetingsWithGroupId.Add(meet);
            }
            if (count != null && gettAllGroupsInMeeting.Count() >= count.Value)
                return _mapper.Map<IEnumerable<MeetingDto>>(allMetingsWithGroupId).Take(count.Value);

            return _mapper.Map<IEnumerable<MeetingDto>>(allMetingsWithGroupId);
        }

        public IEnumerable<T> GettAllChecked<T>(int meetingId, int? trainerId = null)
        {
            var response = _context.MeetingInternGroups.Where(tgi => tgi.Deleted == false && tgi.MeetingId == meetingId);
            if (typeof(T) == typeof(ObjectInternDto))
            {
                if (meetingId == -1)
                {
                    var obj = new List<ObjectInternDto>();
                    foreach (var intern in _mapper.Map<IEnumerable<PersonDto>>(_context.People.Where(p => p.Deleted == false && p.Status == "Intern")))
                    {
                        obj.Add(new ObjectInternDto
                        {
                            InternId = intern.PersonId,
                            FirstName = intern.FirstName,
                            LastName = intern.LastName,
                            Username = intern.Username,
                            IsChecked = false
                        });
                    }
                    return (IEnumerable<T>)obj;
                }
                var interns = response
                     .Where(r => r.InternId != null)
                     .Include(tgi => tgi.Intern)
                     .Select(tgi => new ObjectInternDto
                     {
                         InternId = tgi.InternId.Value,
                         FirstName = tgi.Intern.FirstName,
                         LastName = tgi.Intern.LastName,
                         Username = tgi.Intern.Username,
                         IsChecked = true
                     }).ToList();
                foreach (var intern in _mapper.Map<IEnumerable<PersonDto>>(_context.People.Where(p => p.Deleted == false && p.Status == "Intern")))
                {
                    if (interns.FirstOrDefault(i => i.InternId == intern.PersonId) == null)
                    {
                        interns.Add(new ObjectInternDto
                        {
                            InternId = intern.PersonId,
                            FirstName = intern.FirstName,
                            LastName = intern.LastName,
                            Username = intern.Username,
                            IsChecked = false
                        });
                    }
                }
                return (IEnumerable<T>)interns;
            }
            else if (typeof(T) == typeof(ObjectGroupDto))
            {
                if (meetingId == -1)
                {
                    var obj = new List<ObjectGroupDto>();
                    foreach (var gr in _mapper.Map<IEnumerable<GroupDto>>(_context.Groups.Where(p => p.Deleted == false && p.TrainerId == trainerId.Value)))
                    {
                        obj.Add(new ObjectGroupDto
                        {
                            GroupId = gr.GroupId,
                            GroupName = gr.GroupName,
                            IsChecked = false
                        });
                    }
                    return (IEnumerable<T>)obj;
                }
                var groups = response
                        .Where(r => r.GroupId != null)
                        .Include(tgi => tgi.Group)
                        .Select(tgi => new ObjectGroupDto
                        {
                            GroupId = tgi.GroupId.Value,
                            GroupName = tgi.Group.GroupName,
                            IsChecked = true
                        }).ToList();
                foreach (var gr in _mapper.Map<IEnumerable<GroupDto>>(_context.Groups.Where(p => p.Deleted == false && p.TrainerId == trainerId.Value)))
                {
                    if (groups.FirstOrDefault(i => i.GroupId == gr.GroupId) == null)
                    {
                        groups.Add(new ObjectGroupDto
                        {
                            GroupId = gr.GroupId,
                            GroupName = gr.GroupName,
                            IsChecked = false
                        });
                    }
                }
                return (IEnumerable<T>)groups;
            }
            return null;
        }
    }
}
