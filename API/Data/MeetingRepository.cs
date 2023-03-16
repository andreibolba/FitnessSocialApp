using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

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

        public void Create(MeetingDto meeting)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MeetingDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MeetingDto> GetAllByPersonId(int personId)
        {
            throw new NotImplementedException();
        }

        public MeetingDto GetMeetingById(int id)
        {
            throw new NotImplementedException();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(MeetingDto meeting)
        {
            throw new NotImplementedException();
        }
    }
}
