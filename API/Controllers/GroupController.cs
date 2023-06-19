using API.Data;
using API.Dtos;
using API.Interfaces;
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
        private readonly IPhotoService _photoService;
        private readonly IPictureRepository _pictureRepository;

        public GroupController(IGroupRepository groupRepository, ITestRepository testRepository, IPhotoService photoService, IPictureRepository pictureRepository)
        {
            _groupRepository = groupRepository;
            _testRepository = testRepository;
            _photoService = photoService;
            _pictureRepository = pictureRepository;
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

        [HttpPost("picture/add/{groupId:int}")]
        public ActionResult AddPicture(int groupId)
        {
            var group = _groupRepository.GetGroupById(groupId);

            if (group == null)
            {
                return NotFound("User is now found");
            }

            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No image received!");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File is not a valid image!");

            if (group.PictureId != null)
            {
                var res = _photoService.DeleleteImage(_pictureRepository.GetById(group.PictureId.Value).PublicId);
                if (res.Error != null) return BadRequest(res.Error.Message);

            }

            var result = _photoService.AddImage(file, "InternHub/Groups");

            if (result.Error != null) return BadRequest(result.Error.Message);

            var pic = _pictureRepository.AddPicture(new PictureDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            });

            if (pic == null)
                BadRequest("Internal Server Error");

            group.PictureId = pic.PictureId;

            var groupWithPic = _groupRepository.Update(group);

            return groupWithPic!=null ? Ok(groupWithPic) : BadRequest("Internal Server Error");
        }
    }
}