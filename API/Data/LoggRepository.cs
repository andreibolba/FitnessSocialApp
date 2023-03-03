using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class LoggRepository : ILoggRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public LoggRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(LoggingDto logDto)
        {
            _context.Loggings.Add(_mapper.Map<Logging>(logDto));
        }

        public void Delete(int logId)
        {
            var loggings = _context.Loggings.SingleOrDefault(log => log.LoggingId == logId && log.Deleted == false);
            loggings.Deleted = true;
            _context.Update(loggings);
        }

        public IEnumerable<LoggingDto> GetAllLoggs()
        {
            return _mapper.Map<IEnumerable<LoggingDto>>(_context.Loggings.ToList());
        }

        public LoggingDto GetLogById(int id)
        {
            return _mapper.Map<LoggingDto>(GetAllLoggs().SingleOrDefault(l => l.LogId == id));
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(LoggingDto logDto)
        {
            _context.Loggings.Update(_mapper.Map<Logging>(logDto));
        }
    }
}
