using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class MeetingController : BaseAPIController
    {
        private readonly IMeetingRepository _repository;

        public MeetingController(IMeetingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult GetAllMeetings()
        {
            return Ok(_repository.GetAll());
        }

        [HttpGet("{meetId:int}")]
        public ActionResult GetOneMeeting(int meetId)
        {
            return Ok(_repository.GetMeetingById(meetId));
        }

        [HttpGet("{id:int}/{status}")]
        public ActionResult GetMeetingForPerson(int id, string status)
        {
            switch(status)
            {
                case "inter":
                    return Ok(_repository.GetAllByInternId(id));
                case "trainer":
                    return Ok(_repository.GetAllByTrainerId(id));
                case "group":
                    return Ok(_repository.GetAllByGroupId(id));
                default:
                    return BadRequest("Invalid option");

            }
        }

        [HttpPost("add")]
        public ActionResult Create([FromBody] MeetingDto meeting)
        {
            var meet = _repository.Create(meeting);
            return _repository.SaveAll() ? Ok(meet) : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult Update([FromBody] MeetingDto meeting)
        {
            _repository.Update(meeting);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{meetId:int}")]
        public ActionResult Delete(int meetId)
        {
            _repository.Delete(meetId);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
