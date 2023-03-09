using API.Data;
using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class GroupController : BaseAPIController
    {
        private readonly IGroupRepository _repository;

        public GroupController(IGroupRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetGroups()
        {
            return Ok(_repository.GetAllGroups());
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            _repository.Create(group);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{groupId:int}")]
        public ActionResult DeleteGroup(int groupId)
        {
            _repository.Delete(groupId);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult UpdateGroup([FromBody] GroupDto group)
        {
            _repository.Update(group);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }

        [HttpGet("tests/{groupId:int}")]
        public ActionResult GetAllGroupTest(int groupId)
        {
            return Ok(_repository.GetAllTestsFromGroup(groupId));
        }
    }
}