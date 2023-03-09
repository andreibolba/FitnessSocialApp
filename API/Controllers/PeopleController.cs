using System.Security.Cryptography;
using System.Text;
using API.Dtos;
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

        public PeopleController(IPersonRepository personRepository, IPasswordLinkRepository passwordLinkRepository)
        {
            _personRepository = personRepository;
            _passwordLinkRepository = passwordLinkRepository;
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
                Time = password.Time.AddHours(2),
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


            if (Utils.Utils.SendEmail(details))
                return Ok();
            else
                return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("link")]
        public ActionResult IsValidLink([FromBody] ForgotPasswordDto password)
        {
            var link = _passwordLinkRepository.GetById(password.LinkId);
            if (link == null)
                return BadRequest("Invalid link");
            password.Time.AddHours(2);
            if (link.Time < password.Time)
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
            _passwordLinkRepository.Delete(link);
            using var hmac = new HMACSHA512();
            person.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Password));
            person.PasswordSalt = hmac.Key;
            _personRepository.Update(person);
            if (_personRepository.SaveAll() && _passwordLinkRepository.SaveAll())
                return Ok();
            return BadRequest("Internal Serve Error!");
        }

        [HttpPost("delete/{personId:int}")]
        public ActionResult DeleteAccount(int personId)
        {
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

            _personRepository.Update(person);
            return _personRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error"); ;
        }

        [HttpGet("tests/{personId:int}")]
        public ActionResult GetAllInternTest(int personId)
        {
            return Ok(_personRepository.GetAllInternTests(personId));
        }
    }
}