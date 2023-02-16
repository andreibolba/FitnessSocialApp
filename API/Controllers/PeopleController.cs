using API.Dtos;
using API.Models;
using API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PeopleController : BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;

        public PeopleController(InternShipAppSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LoggedPersonDto>> GetPeople()
        {
            var result = _context.People.ToList().Where(g => g.Deleted == false);
            var resultToReturn = new List<LoggedPersonDto>(0);

            foreach (var re in result)
            {
                resultToReturn.Add(new LoggedPersonDto
                {
                    PersonId = re.PersonId,
                    FirstName = re.FirstName,
                    LastName = re.LastName,
                    Email = re.Email,
                    Username = re.Username,
                    Status = re.Status,
                    BirthDate = re.BirthDate
                });
            }

            return resultToReturn;
        }

        [HttpGet("{id:int}")]
        public ActionResult<LoggedPersonDto> GetPerson(int id)
        {
            var person = _context.People.SingleOrDefault(p => (p.PersonId == id && p.Deleted == false));
            if (person == null)
                return NoContent();
            var personToSend = new LoggedPersonDto()
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Username = person.Username,
                Status = person.Status,
                BirthDate = person.BirthDate
            };
            return personToSend;
        }

        [HttpGet("{username}")]
        public ActionResult<LoggedPersonDto> GetPersonByUsername(string username)
        {
            var person = _context.People.SingleOrDefault(user => (user.Username == username && user.Deleted == false));
            if (person == null)
                return NoContent();
            var personToSend = new LoggedPersonDto()
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Username = person.Username,
                Status = person.Status,
                BirthDate = person.BirthDate
            };
            return personToSend;
        }

        [HttpPost("forgot")]
        public ActionResult<PersonDto> Register([FromBody] ForgotPasswordDto password)
        {
            EmailFields details = new EmailFields
            {
                EmailTo = password.Email,
                EmailSubject = "Recovery password link",
                EmailBody = "Hello! \n\n" +
                "Forgot your password? No worries. It happens to everyone. We’ve made it easy for you to access Intern Hub again.\n\n" +
                "You can reset your password immediately by clicking here or pasting the following link in your browser:\n\n" +
                "https://localhost:4200/recovery/" + password.Username + "\n\n" +
                "Link is available 1 hour!\n\n"+
                "Cheers,\n" +
                "The Internhub Team!"

            };
            if (Utils.Utils.SendEmail(details))
                return Ok();
            else
                return BadRequest();
        }

    }
}