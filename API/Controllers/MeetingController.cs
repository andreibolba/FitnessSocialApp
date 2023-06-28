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

        [HttpGet("{id:int}/{status}/{count?}")]
        public ActionResult GetMeetingForPerson(int id, string status, int? count=null)
        {
            switch(status)
            {
                case "trainer":
                    return Ok(_repository.GetAllByTrainerId(id, count));
                case "intern":
                    return Ok(_repository.GetAllByInternId(id, count));
                case "group":
                    return Ok(_repository.GetAllByGroupId(id, count));
                default:
                    return BadRequest("Invalid option");

            }
        }

        [HttpPost("add")]
        public ActionResult Create([FromBody] MeetingDto meeting)
        {
            var meet = _repository.Create(meeting);
            return meet!=null ? Ok(_repository.GetMeetingById(meet.MeetingId)) : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult Update([FromBody] MeetingDto meeting)
        {
            var meet = _repository.Update(meeting);
            return meet != null ? Ok(_repository.GetMeetingById(meet.MeetingId)) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{meetId:int}")]
        public ActionResult Delete(int meetId)
        {
            _repository.Delete(meetId);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet("all/{meetingId:int}/{option}/{trainerId?}")]
        public ActionResult UpdateTestAttributions(int meetingId, string option,int? trainerId=null)
        {
            switch (option)
            {
                case "interns":
                    return Ok(_repository.GettAllChecked<ObjectInternDto>(meetingId));
                case "groups":
                    return Ok(_repository.GettAllChecked<ObjectGroupDto>(meetingId,trainerId.Value));
                default:
                    return BadRequest("Invalid option!");
            }
        }
    }
}
