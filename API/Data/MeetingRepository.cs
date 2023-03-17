using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using System.Text.RegularExpressions;

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
            _context.Meetings.Update(meet);
        }

        public IEnumerable<MeetingDto> GetAll()
        {
            var result = _context.Meetings.Where(m => m.Deleted == false);
            return _mapper.Map<IEnumerable<MeetingDto>>(result);
        }

        public IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count=null)
        {
            if (count != null)
                return GetAll().Where(m => m.TrainerId == trainerId).Take(count.Value);
            return GetAll().Where(m => m.TrainerId == trainerId);
        }

        public IEnumerable<MeetingDto> GetAllByGroupId(int groupId, int? count = null)
        {
            if(count!=null)
                return GetAll().Where(m => m.GroupId == groupId).Take(count.Value);
            return GetAll().Where(m => m.GroupId == groupId);
        }

        public IEnumerable<MeetingDto> GetAllByInternId(int internId, int? count = null)
        {
            if (count != null)
                return GetAll().Where(m => m.InternId == internId).Take(count.Value);
            return GetAll().Where(m => m.InternId == internId);
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
    }
}
