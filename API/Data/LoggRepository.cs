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
            Logging log = _mapper.Map<Logging>(logDto);
            _context.Loggings.Add(log);
        }

        public void Delete(int logId)
        {
            var loggings = _context.Loggings.SingleOrDefault(log => log.LoggingId == logId && log.Deleted == false);
            loggings.Deleted = true;
            _context.Update(loggings);
        }

        public IEnumerable<LoggingDto> GetAllLoggs()
        {
            var loggings = _context.Loggings.ToList();
            var loggingsToReturn = _mapper.Map<IEnumerable<LoggingDto>>(loggings);
            return loggingsToReturn;
        }

        public LoggingDto GetLogById(int id)
        {
            var loggings = _context.Loggings.ToList().Where(log=>log.LoggingId==id&&log.Deleted==false);
            var loggingsToReturn = _mapper.Map<LoggingDto>(loggings);
            return loggingsToReturn;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(LoggingDto logDto)
        {
            Logging log = _mapper.Map<Logging>(logDto);

            _context.Loggings.Update(log);
        }
    }
}
