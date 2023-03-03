using API.Dtos;
using API.Interfaces;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace API.Data
{
    public class PersonRepository : IPersonRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _token;

        public PersonRepository(InternShipAppSystemContext context, IMapper mapper, ITokenService token)
        {
            _context = context;
            _mapper = mapper;
            _token = token;
        }

        public bool Create(PersonDto person)
        {
            using var hmac = new HMACSHA512();
            person.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Utils.Utils.CreatePassword(20)));
            person.PasswordSalt = hmac.Key;

            Person newPerson = _mapper.Map<Person>(person);

            _context.People.Add(newPerson);

            PasswordkLink link = new PasswordkLink
            {
                PersonUsername = newPerson.Username,
                Time = person.Created.AddHours(2),
                Deleted = false
            };

            _context.PasswordkLinks.Add(link);

            Utils.EmailFields details = new Utils.EmailFields
            {
                EmailTo = person.Email,
                EmailSubject = "Welcome to Intenr Hub",
                EmailBody = "Hello! \n\n" +
                           "We are glad to have you in our team as " + newPerson.Status + ". You have to complete your account by seeting the password!\n\n" +
                           "You can set your password immediately by clicking here or pasting the following link in your browser:\n\n" +
                           "https://localhost:4200/recovery/" + link.PasswordLinkId + "\n\n" +
                           "Link is available 1 hour!\n\n" +
                           "Set your password and explore the InternHub!\n\n" +
                           "Cheers,\n" +
                           "The Internhub Team!"

            };

            return Utils.Utils.SendEmail(details);
        }

        public void Delete(int personId)
        {
            var personToDelete = _context.People.SingleOrDefault(p => p.PersonId == personId);
            personToDelete.Deleted = true;
            _context.People.Update(personToDelete);
            var allGroup = _context.InternGroups.ToList().Where(gi => gi.InternId == personId);
            foreach (var a in allGroup)
            {
                a.Deleted = true;
                _context.InternGroups.Update(a);
            }
        }

        public IEnumerable<PersonDto> GetAllPerson()
        {
            var persons = _context.People.ToList().Where(g => g.Deleted == false);
            var personsToReturn = _mapper.Map<IEnumerable<PersonDto>>(persons);
            return personsToReturn;
        }

        public PersonDto GetPersonById(int id)
        {
            var person = _context.People.SingleOrDefault(p => (p.PersonId == id && p.Deleted == false));
            if (person == null)
                return null;
            var personToSend = _mapper.Map<PersonDto>(person);
            return personToSend;
        }

        public PersonDto GetPersonByUsername(string username)
        {
            var person = _context.People.SingleOrDefault(user => (user.Username == username && user.Deleted == false));
            if (person == null)
                return null;
            var personToSend = _mapper.Map<PersonDto>(person);
            return personToSend;
        }

        public PersonDto LogIn(string email,string password)
        {
            var loggedPerson = _context.People.SingleOrDefault(p => (p.Email == email && p.Deleted == false));
            if (loggedPerson == null)
                return null;
            using var hmac = new HMACSHA512(loggedPerson.PasswordSalt);
            var compputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < compputedHash.Length; i++)
            {
                if (compputedHash[i] != loggedPerson.PasswordHash[i])
                    return null;
            }
            return new PersonDto
            {
                Username = loggedPerson.Username,
                Token = _token.CreateToken(loggedPerson)
            };
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(PersonDto person)
        {
            Person personToUpdate = _mapper.Map<Person>(person);
            _context.People.Update(personToUpdate);
        }

        public IEnumerable<PersonDto> GetAllAdmins()
        {
            return this.GetAllPerson().Where(g => g.Status == "Admin");
        }

        public IEnumerable<PersonDto> GetAllInterns()
        {
            return this.GetAllPerson().Where(g => g.Status == "Interns");
        }

        public IEnumerable<PersonDto> GetAllTrainers()
        {
            return this.GetAllPerson().Where(g => g.Status == "Trainers");
        }

        public PersonDto GetPersonByEmail(string email)
        {
            var person = _context.People.SingleOrDefault(user => (user.Email == email && user.Deleted == false));
            if (person == null)
                return null;
            var personToSend = _mapper.Map<PersonDto>(person);
            return personToSend;
        }
    }
}
