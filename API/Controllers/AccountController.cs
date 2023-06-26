using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AccountController : BaseAPIController
    {

        private readonly IPersonRepository _repository;

        public AccountController(IPersonRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] PersonDto person)
        {

            if (_repository.GetPersonByUsername(person.Username)!=null)
                return BadRequest("Username exists!");

            if (_repository.GetPersonByEmail(person.Email) != null)
                return BadRequest("Email exists!");

            using var hmac = new HMACSHA512();
            person.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(Utils.Utils.CreatePassword(20)));
            person.PasswordSalt=hmac.Key;

            var res = _repository.Create(person);
            if (res!=null)
                return Ok(res);
            else
                return BadRequest("Account was created, but the mail was not sent!");
        }

        [HttpPost("login")]
        public ActionResult<PersonDto> LogIn([FromBody] PersonDto person)
        {
            var loggedPerson = _repository.LogIn(person.Email,person.Password);
            if (loggedPerson == null)
                return Unauthorized("Invalid details!");
            return loggedPerson;
        }
    }
}