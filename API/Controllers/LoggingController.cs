using API.Dtos;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LoggingController:BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;
        private readonly ITokenService _tokenService;

        public LoggingController(InternShipAppSystemContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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
        public async Task<ActionResult<IEnumerable<Logging>>> GetLog(){
            return await _context.Loggings.ToListAsync();
        }
    }
}