using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LoggingController:BaseAPIController
    {
        private readonly ILoggRepository _repository;

        public LoggingController(ILoggRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("log")]
        public ActionResult Log([FromBody] LoggingDto logDto){
            _repository.Create(logDto);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet]
        public ActionResult<IEnumerable<LoggingDto>> GetLoggings(){
            return Ok(_repository.GetAllLoggs());
        }
    }
}