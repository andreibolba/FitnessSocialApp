using API.Dtos;
using API.Models;
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
            var result = _context.People.ToList().Where(g=>g.Deleted==false);
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
            var person =_context.People.SingleOrDefault(p=>(p.PersonId == id && p.Deleted==false));
            if(person==null)
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
            var person = _context.People.SingleOrDefault(user =>( user.Username == username && user.Deleted==false));
            if(person==null)
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
    }
}