using API.Data;
using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var group = _groupRepository.GetGroupById(groupId);
            return Ok(new
            {
                Tests = _testRepository.GetTestForGroup(groupId),
                Participants = _groupRepository.GetAllParticipants(groupId).Count(),
                Tasks = 0,
                Challanges = 0,
                Group = _groupRepository.GetGroupById(groupId),
            });
        }

        [HttpGet("image/{groupId:int}")]
        public ActionResult GetImageForGroup(int groupId)
        {
            var group = _groupRepository.GetGroupById(groupId);
            var picture = File(group.Picture, "image/jpeg");
            return Ok(new
            {
                Picture = group.Picture.Length <= 0 ? null : picture
            });
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            _groupRepository.Create(group);
            return _groupRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
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
            _groupRepository.Update(group);
            return _groupRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }

        [HttpPost("upload/image/{groupId:int}")]
        public ActionResult UpdateImage(int groupId)
        {
            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No image received!");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File is not a valid image!");

            long fileSize = file.Length;

            if (fileSize > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] picture = stream.ToArray();

                    _groupRepository.SetPicture(groupId, picture);

                    return _groupRepository.SaveAll() ? Ok("The picture was set!") : BadRequest("Internal Server Error");
                }
            }

            return BadRequest("Internal Server Error");
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