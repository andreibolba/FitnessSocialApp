using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LoggingController:BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public LoggingController(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("log")]
        public async void Log([FromBody] LoggingDto logDto){
            Logging log=new Logging{
                LogType=logDto.LogType,
                LogMessage=logDto.LogMessage,
                PersonUsername=logDto.PersonUsername,
                DateOfLog=logDto.DateOfLog,
                Deleted=false
            };

            _context.Loggings.Add(log);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public ActionResult<IEnumerable<LoggingDto>> GetLoggings(){
            var logger = _context.Loggings.ToList();
            var loggerToReturn = _mapper.Map<IEnumerable<LoggingDto>>(logger);
            return Ok(loggerToReturn);
        }
    }
}