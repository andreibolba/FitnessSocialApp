using API.Data;
using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class GroupController : BaseAPIController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ITestRepository _testRepository;

        public GroupController(IGroupRepository groupRepository, ITestRepository testRepository)
        {
            _groupRepository = groupRepository;
            _testRepository = testRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetGroups()
        {
            return Ok(_groupRepository.GetAllGroups());
        }

        [HttpGet("{groupId:int}")]
        public ActionResult GetGroupById(int groupId)
        {
            return Ok(new
            {
                Tests = _testRepository.GetTestForGroup(groupId),
                Participants = _groupRepository.GetAllParticipants(groupId).Count(),
                Tasks = 0,
                Challanges = 0,
                Group = _groupRepository.GetGroupById(groupId),
            });
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            var res = _groupRepository.Create(group);
            return res!=null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{groupId:int}")]
        public ActionResult DeleteGroup(int groupId)
        {
            _groupRepository.Delete(groupId);
            return _groupRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult UpdateGroup([FromBody] GroupDto group)
        {
            var res = _groupRepository.Update(group);
            return res!=null ? Ok(res) : BadRequest("Internal Server Error"); ;
        }

        [HttpGet("tests/{groupId:int}")]
        public ActionResult GetAllGroupTest(int groupId)
        {
            return Ok(_groupRepository.GetAllTestsFromGroup(groupId));
        }

        [HttpGet("participants/{groupId:int}")]
        public ActionResult<IEnumerable<PersonDto>> GetAllPersonInGroup(int groupId)
        {
            return Ok(_groupRepository.GetAllParticipants(groupId));
        }
    }
}