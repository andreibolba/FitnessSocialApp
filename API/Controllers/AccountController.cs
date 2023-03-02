using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseAPIController
    {

        private readonly InternShipAppSystemContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(InternShipAppSystemContext context, ITokenService tokenService, IMapper mapper)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] PersonDto person)
        {

            if (Utils.Utils.UsernameExists(person.Username, _context))
                return BadRequest("Username exists!");

            if (Utils.Utils.EmailExists(person.Email, _context))
                return BadRequest("Email exists!");

            using var hmac = new HMACSHA512();
            person.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(Utils.Utils.CreatePassword(20)));
            person.PasswordSalt=hmac.Key;

            Person newPerson = _mapper.Map<Person>(person);

            _context.People.Add(newPerson);
            await _context.SaveChangesAsync();

            PasswordkLink link = new PasswordkLink
            {
                PersonUsername = newPerson.Username,
                Time = person.Created.AddHours(2),
                Deleted = false
            };

            _context.PasswordkLinks.Add(link);
            _context.SaveChanges();

            Utils.EmailFields details = new Utils.EmailFields
            {
                EmailTo = person.Email,
                EmailSubject = "Welcome to Intenr Hub",
                EmailBody = "Hello! \n\n" +
                           "We are glad to have you in our team as "+ newPerson.Status +". You have to complete your account by seeting the password!\n\n" +
                           "You can set your password immediately by clicking here or pasting the following link in your browser:\n\n" +
                           "https://localhost:4200/recovery/" + link.PasswordLinkId + "\n\n" +
                           "Link is available 1 hour!\n\n" +
                           "Set your password and explore the InternHub!\n\n"+
                           "Cheers,\n" +
                           "The Internhub Team!"

            };

            if (Utils.Utils.SendEmail(details))
                return Ok();
            else
                return BadRequest("Account was created, but the mail was not sent!");
        }

        [HttpPost("login")]
        public async Task<ActionResult<PersonDto>> LogIn([FromBody] PersonDto person)
        {
            var loggedPerson = await _context.People.SingleOrDefaultAsync(p => (p.Email == person.Email && p.Deleted == false));
            if (loggedPerson == null)
                return Unauthorized("Invalid email!");
            using var hmac = new HMACSHA512(loggedPerson.PasswordSalt);
            var compputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(person.Password));
            for (int i = 0; i < compputedHash.Length; i++)
            {
                if (compputedHash[i] != loggedPerson.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }
            return new PersonDto
            {
                Username = loggedPerson.Username,
                Token = _tokenService.CreateToken(loggedPerson)
            };
        }
    }
}