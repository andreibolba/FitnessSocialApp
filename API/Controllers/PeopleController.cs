using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Interfaces;
using API.Interfaces.Repository;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PeopleController : BaseAPIController
    {
        private readonly IPersonRepository _personRepository;
        private readonly IPasswordLinkRepository _passwordLinkRepository;
        private readonly IPhotoService _photoService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupChatRepository _groupChatRepository;

        public PeopleController(IPersonRepository personRepository, IPasswordLinkRepository passwordLinkRepository, IPhotoService photoService, IPictureRepository pictureRepository, IGroupRepository groupRepository, IGroupChatRepository groupChatRepository)
        {
            _personRepository = personRepository;
            _passwordLinkRepository = passwordLinkRepository;
            _photoService = photoService;
            _pictureRepository = pictureRepository;
            _groupRepository = groupRepository;
            _groupChatRepository = groupChatRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonDto>> GetPeople()
        {
            return Ok(_personRepository.GetAllPerson());
        }

        [HttpGet("{id:int}")]
        public ActionResult<PersonDto> GetPerson(int id)
        {
            var person = _personRepository.GetPersonById(id);
            if (person == null)
                return NoContent();
            return Ok(person);
        }

        [HttpGet("{username}")]
        public ActionResult<PersonDto> GetPersonByUsername(string username)
        {
            var person = _personRepository.GetPersonByUsername(username);
            if (person == null)
                return NoContent();
            return Ok(person);
        }

        [AllowAnonymous]
        [HttpPost("forgot")]
        public ActionResult ForgotPassword([FromBody] ForgotPasswordDto password)
        {
            var person = _personRepository.GetPersonByEmail(password.Email);
            if(person == null)
            {
                return BadRequest("Email is not recognized!");
            }
            PasswordkLink link = new PasswordkLink
            {
                PersonUsername = person.Username,
                Time = DateTime.Now.AddHours(1),
                Deleted = false
            };

            _passwordLinkRepository.Create(link);

            if (_passwordLinkRepository.SaveAll() == false)
                return BadRequest("Internal Server error!");

            EmailFields details = new EmailFields
            {
                EmailTo = person.Email,
                EmailSubject = "Recovery password link",
                EmailBody = "Hello! \n\n" +
                "Forgot your password? No worries. It happens to everyone. We’ve made it easy for you to access Intern Hub again.\n\n" +
                "You can reset your password immediately by clicking here or pasting the following link in your browser:\n\n" +
                "https://localhost:4200/recovery/" + link.PasswordLinkId + "\n\n" +
                "Link is available 1 hour!\n\n" +
                "Cheers,\n" +
                "The Internhub Team!"

            };

            var res = Utils.Utils.SendEmail(details);
            if (res==string.Empty)
                return Ok();
            else
                return BadRequest(res);
        }

        [AllowAnonymous]
        [HttpPost("link")]
        public ActionResult IsValidLink([FromBody] ForgotPasswordDto password)
        {
            var link = _passwordLinkRepository.GetById(password.LinkId);
            if (link == null)
                return BadRequest("Invalid link");
            if (link.Time < DateTime.Now)
            {
                _passwordLinkRepository.Delete(link);
                if (_passwordLinkRepository.SaveAll() == false)
                    return BadRequest("Internal Serve Error!");
                return BadRequest("Link is expired!");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult PasswordReset([FromBody] ForgotPasswordDto password)
        {
            var link = _passwordLinkRepository.GetById(password.LinkId);
            var person = _personRepository.GetPersonByUsername(link.PersonUsername);

            if (link == null && person == null)
                return BadRequest();
            
            person.Password = password.Password;
            var res = _personRepository.Update(person);
            if(res==null)
                return BadRequest("Internal Serve Error!");
            var resPas = _passwordLinkRepository.Delete(link);
            if (resPas!=null)
                return Ok();
            return BadRequest("Internal Serve Error!");
        }

        [HttpPost("delete/{personId:int}")]
        public ActionResult DeleteAccount(int personId)
        {
            var gropus = _groupRepository.GetAllGroups().Where(g=>g.TrainerId==personId).Any();
            var gropusChat = _groupChatRepository.GetAllGroupChats().Where(g=>g.AdminId == personId).Any();
            if (gropus)
                return BadRequest("You can't delete a person that is admin in a group!");
            if (gropus)
                return BadRequest("You can't delete a person that is admin in a group chat!");
            _personRepository.Delete(personId);
            return _personRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpPost("update")]
        public ActionResult UpdateAccount([FromBody] PersonDto person)
        {
            var personCheck = _personRepository.GetPersonById(person.PersonId);
            if (_personRepository.GetPersonByUsername(person.Username)!= null && (person.Username != personCheck.Username))
                return BadRequest("Username exists!");

            if (_personRepository.GetPersonByEmail(person.Email)!=null && (person.Email != personCheck.Email))
                return BadRequest("Email exists!");

            var res = _personRepository.Update(person);
            return res!=null? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpGet("tests/{personId:int}")]
        public ActionResult GetAllInternTest(int personId)
        {
            return Ok(_personRepository.GetAllInternTests(personId));
        }

        [HttpPost("picture/add/{personId:int}")]
        public ActionResult AddPicture(int personId)
        {
            var user = _personRepository.GetPersonById(personId);

            if (user == null)
            {
                return NotFound("User is now found");
            }

            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No image received!");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("File is not a valid image!");

            if (user.PictureId != null)
            {
                var res = _photoService.DeleleteImage(_pictureRepository.GetById(user.PictureId.Value).PublicId);
                if (res.Error != null) return BadRequest(res.Error.Message);

            }

            var result = _photoService.AddImage(file,"InternHub/Users");

            if (result.Error != null) return BadRequest(result.Error.Message);

            var pic = _pictureRepository.AddPicture(new PictureDto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            });

            if(pic == null)
                BadRequest("Internal Server Error");

            user.PictureId = pic.PictureId;

            _personRepository.Update(user);

            return _personRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}