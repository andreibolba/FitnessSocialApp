using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Models;
using API.Utils;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PeopleController : BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public PeopleController(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonDto>> GetPeople()
        {
            var result = _context.People.ToList().Where(g => g.Deleted == false);
            var resultToReturn = _mapper.Map<IEnumerable<PersonDto>>(result);
            return Ok(resultToReturn);
        }

        [HttpGet("{id:int}")]
        public ActionResult<PersonDto> GetPerson(int id)
        {
            var person = _context.People.SingleOrDefault(p => (p.PersonId == id && p.Deleted == false));
            if (person == null)
                return NoContent();
            var personToSend = _mapper.Map<PersonDto>(person);
            return Ok(personToSend);
        }

        [HttpGet("{username}")]
        public ActionResult<PersonDto> GetPersonByUsername(string username)
        {
            var person = _context.People.SingleOrDefault(user => (user.Username == username && user.Deleted == false));
            if (person == null)
                return NoContent();
            var personToSend = _mapper.Map<PersonDto>(person);
            return Ok(personToSend);
        }

        [AllowAnonymous]
        [HttpPost("forgot")]
        public ActionResult ForgotPassword([FromBody] ForgotPasswordDto password)
        {
            var person = _context.People.SingleOrDefault(user => (user.Email == password.Email && user.Deleted == false));
            PasswordkLink link = new PasswordkLink
            {
                PersonUsername = person.Username,
                Time = password.Time.AddHours(2),
                Deleted = false
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
            var link = _context.PasswordkLinks.SingleOrDefault(link => (link.PasswordLinkId == password.LinkId && link.Deleted == false));
            if (link == null)
                return BadRequest("Invalid link");
            password.Time.AddHours(2);
            if (link.Time < password.Time){
                link.Deleted=true;
                _context.PasswordkLinks.Update(link);
                _context.SaveChanges();
                return BadRequest("Link is expired!");
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public ActionResult PasswordReset([FromBody] ForgotPasswordDto password)
        {
            var link = _context.PasswordkLinks.SingleOrDefault(link => (link.PasswordLinkId == password.LinkId && link.Deleted == false));
            var person = _context.People.SingleOrDefault(p => p.Username == link.PersonUsername && p.Deleted == false);
            if (link == null && person == null)
                return BadRequest();
            link.Deleted = true;
            _context.PasswordkLinks.Update(link);
            using var hmac = new HMACSHA512();
            person.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password.Password));
            person.PasswordSalt = hmac.Key;
            _context.People.Update(person);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("delete/{personId:int}")]
        public ActionResult DeleteAccount(int personId)
        {
            var personToDelete=_context.People.SingleOrDefault(p=>p.PersonId == personId);
            personToDelete.Deleted=true;
            _context.People.Update(personToDelete);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("update")]
        public ActionResult UpdateAccount([FromBody] PersonDto person)
        {
            var personToUpdate=_context.People.SingleOrDefault(p=>p.PersonId == person.PersonId);
            if ( Utils.Utils.UsernameExists(person.Username,_context) && person.Username!=personToUpdate.Username)
                return BadRequest("Username exists!");

            if ( Utils.Utils.EmailExists(person.Email,_context)&& person.Email!=personToUpdate.Email)
                return BadRequest("Email exists!");
                
            personToUpdate.FirstName=person.FirstName;
            personToUpdate.LastName=person.LastName;
            personToUpdate.Email=person.Email;
            personToUpdate.Username=person.Username;
            personToUpdate.Status=person.Status;
            personToUpdate.BirthDate=person.BirthDate;
            _context.People.Update(personToUpdate);
            _context.SaveChanges();
            return Ok();
        }
    }
}