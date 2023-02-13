using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class PeopleController:BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;

        public PeopleController(InternShipAppSystemContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople(){
            return await _context.People.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Person>> GetPerson(int id){
            return await _context.People.FindAsync(id);
        }

        [AllowAnonymous]
        [HttpGet("{username}")]
        public ActionResult<LoggedPersonDto> GetPersonByUsername(string username)
        {
            var person = _context.People.SingleOrDefault(user => user.Username == username) as Person;
            var personToSend = new LoggedPersonDto()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Username = person.Username,
                Status = person.Status,
                BirthDate = person.BirthDate
            };
            return personToSend;
        }
    }
}