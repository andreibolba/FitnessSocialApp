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
            return _mapper.Map<MeetingDto>(meet);
        }

        public void Delete(int id)
        {
            var meet = _mapper.Map<Meeting>(GetMeetingById(id));
            meet.Deleted = true;
            var allMeetingsConnection = _context.MeetingInternGroups.Where(mg => mg.MeetingId == id);
            foreach(var meetCon in allMeetingsConnection)
            {
                meetCon.Deleted = true;
                _context.MeetingInternGroups.Update(meetCon);
            }
            _context.Meetings.Update(meet);
        }

        public IEnumerable<MeetingDto> GetAll()
        {
            var result = _mapper.Map<IEnumerable<MeetingDto>>(_context.Meetings.Where(m => m.Deleted == false));
            foreach(var r in result)
            {
                var allPeopleInMeeting = new List<PersonDto>();
                var allGroups = _context.MeetingInternGroups.Where(mg => mg.Deleted == false&& mg.InternId==null && mg.MeetingId == r.MeetingId).Include(mg=>mg.Group).Select(mg => mg.Group);
                foreach (var gr in _context.MeetingInternGroups.Where(mg => mg.Deleted == false && mg.MeetingId == r.MeetingId).Select(mg => mg.Group))
                {
                    var allPeople = _context.InternGroups.Where(ig => ig.GroupId == gr.GroupId && ig.Deleted == false).Include(ig => ig.Intern).Select(ig => ig.Intern);
                    allPeopleInMeeting.AddRange(_mapper.Map<IEnumerable<PersonDto>>(allPeople));
                }

                var allinterns = _context.MeetingInternGroups.Where(mg => mg.Deleted == false && mg.GroupId == null && mg.MeetingId == r.MeetingId).Include(mg => mg.Intern).Select(mg => mg.Intern);
                foreach(var intern in allinterns)
                {
                    if (allPeopleInMeeting.FirstOrDefault(all => all.PersonId == intern.PersonId) == null)
                        allPeopleInMeeting.Add(_mapper.Map<PersonDto>(intern));
                }

                r.AllPeopleInMeeting = allPeopleInMeeting;
            }
            return result;
        }

        public IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count=null)
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
        }

        public IEnumerable<MeetingDto> GetFutureMeetings()
        {
            return GetAll().Where(r => Utils.Utils.IsValidMeeting(r)==true).OrderBy(m => m.MeetingStartTime).ThenBy(m => m.MeetingFinishTime);
        }

        //op-1=>intern
        //op-2=>group
        public bool CreateUpdateDelete(string obj, int meetingId, int op)
        {
            var result = op == 1
               ? _context.MeetingInternGroups.Where(t => t.Deleted == false && t.MeetingId == meetingId && t.GroupId == null)
               : _context.MeetingInternGroups.Where(t => t.Deleted == false && t.MeetingId == meetingId && t.InternId == null);
            List<int> idList = Utils.Utils.FromStringToInt(obj);
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

        public IEnumerable<MeetingDto> GetAllByInternId(int internId, int? count = null)
        {
            List<MeetingDto> allMeetingByPerson = new List<MeetingDto>();
            var allPeople = _context.InternGroups.Where(ig => ig.InternId==internId && ig.Deleted == false);
            foreach(var person in allPeople)
            {
                var allMeetings=GetAllByGroupId(person.GroupId);
                foreach(var meet in allMeetings)
                {
                    if (allMeetingByPerson.FirstOrDefault(m => m.MeetingId == meet.MeetingId) == null)
                        allMeetingByPerson.Add(meet);
                }
            }

            var allMeetingWithPerson = _context.MeetingInternGroups.Where(m => m.Deleted == false && m.GroupId == null).Include(m => m.Meeting).Select(m => m.Meeting);
            foreach(var meet in allMeetingByPerson)
                if (allMeetingByPerson.FirstOrDefault(m => m.MeetingId == meet.MeetingId) == null)
                    allMeetingByPerson.Add(meet);

            if (count != null && allMeetingByPerson.Count() >= count.Value)
                return allMeetingByPerson.Take(count.Value);

            return allMeetingByPerson;
        }

        public IEnumerable<MeetingDto> GetAllByGroupId(int groupId, int? count = null)
        {
            var gettAllGroupsInMeeting = _context.MeetingInternGroups.Where(mg => mg.GroupId == groupId && mg.Deleted == false).Include(mg=>mg.Meeting).Select(mg=>mg.Meeting);
            if (count != null && gettAllGroupsInMeeting.Count() >= count.Value)
                return _mapper.Map<IEnumerable<MeetingDto>>(gettAllGroupsInMeeting).Take(count.Value);

            return _mapper.Map<IEnumerable<MeetingDto>>(gettAllGroupsInMeeting);
        }
    }
}
