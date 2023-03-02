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
        public void Log([FromBody] LoggingDto logDto){
            Logging log = _mapper.Map<Logging>(logDto);

            _context.Loggings.Add(log);
            _context.SaveChanges();
        }

        [HttpGet]
        public ActionResult<IEnumerable<LoggingDto>> GetLoggings(){
            var loggerToReturn = _mapper.Map<IEnumerable<LoggingDto>>( _context.Loggings.ToList());
            return Ok(loggerToReturn);
        }
    }
}