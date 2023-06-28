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
        private readonly IChallengeRepository _chalengeRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IPhotoService _photoService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IPersonRepository _personRepository;

        public GroupController(IGroupRepository groupRepository, 
            ITestRepository testRepository, 
            IChallengeRepository chalengeRepository, 
            ITaskRepository taskRepository, 
            IPhotoService photoService, 
            IPictureRepository pictureRepository, 
            IPersonRepository personRepository)
        {
            _groupRepository = groupRepository;
            _testRepository = testRepository;
            _chalengeRepository = chalengeRepository;
            _taskRepository = taskRepository;
            _photoService = photoService;
            _pictureRepository = pictureRepository;
            _personRepository = personRepository;
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
                Challenges = _chalengeRepository.GetAllChallenges().Count(),
                Tasks = _taskRepository.GetAllTasksForGroup(groupId).Count(),
                Group = _groupRepository.GetGroupById(groupId),
            });
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            var res = _groupRepository.Create(group);
            if(res==null)
                return BadRequest("Internal Server Error");
            res.AllInterns = new List<PersonDto>(0);
            res.Trainer = res.TrainerId != null ? _personRepository.GetPersonById(res.TrainerId.Value) : null ;
            return Ok(res);
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
            if (res == null)
                return BadRequest("Internal Server Error");
            res.Trainer = res.TrainerId != null ? _personRepository.GetPersonById(res.TrainerId.Value) : null;
            return Ok(res);
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