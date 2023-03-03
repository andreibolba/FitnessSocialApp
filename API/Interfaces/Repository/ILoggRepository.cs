using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface ILoggRepository
    {
        void Create(LoggingDto logDto);
        IEnumerable<LoggingDto> GetAllLoggs();
        LoggingDto GetLogById(int id);
        void Delete(int logId);
        bool SaveAll();
    }
}
