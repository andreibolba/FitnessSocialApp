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

        [AllowAnonymous]
        [HttpPost("forgot")]
        public ActionResult<PersonDto> ForgotPassword([FromBody] ForgotPasswordDto password)
        {
            var person = _context.People.SingleOrDefault(user => (user.Email == password.Email && user.Deleted == false));
            PasswordkLink link=new PasswordkLink{
                PersonUsername=person.Username,
                Time=password.Time.AddHours(2),
                Deleted=false
            };

            _context.PasswordkLinks.Add(link);
            _context.SaveChanges();
            EmailFields details = new EmailFields
            {
                EmailTo = person.Email,
                EmailSubject = "Recovery password link",
                EmailBody = "Hello! \n\n" +
                "Forgot your password? No worries. It happens to everyone. We’ve made it easy for you to access Intern Hub again.\n\n" +
                "You can reset your password immediately by clicking here or pasting the following link in your browser:\n\n" +
                "https://localhost:4200/recovery/" + link.PasswordLinkId + "\n\n" +
                "Link is available 1 hour!\n\n"+
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
        public ActionResult<PersonDto> IsValidLink([FromBody] ForgotPasswordDto password)
        {
            var link = _context.PasswordkLinks.SingleOrDefault(link => (link.PasswordLinkId == password.LinkId && link.Deleted == false));
            if(link==null)
                return BadRequest();
            password.Time.AddHours(2);
            if(link.Time < password.Time)
                return BadRequest("Link is expired!");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult<PersonDto> PaswwordReset([FromBody] ForgotPasswordDto password)
        {
            var link = _context.PasswordkLinks.SingleOrDefault(link => (link.PersonUsername == "password.Username" && link.Deleted == false));
            if(link==null)
                return BadRequest();
            return Ok();
        }
    }


}