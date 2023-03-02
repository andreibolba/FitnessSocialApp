using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface ILoggRepository
    {
        void Create(Logging log);
        IEnumerable<LoggingDto> GetAllLoggs();
        LoggingDto GetLogById(int id);
        void Update(Logging log);
        void Delete(Logging log);
        bool SaveAll();
    }
}
