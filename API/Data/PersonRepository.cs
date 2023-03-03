using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Interfaces;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

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

            _context.People.Add(_mapper.Map<Person>(person));

            PasswordkLink link = new PasswordkLink
            {
                PersonUsername = person.Username,
                Time = person.Created.AddHours(2),
                Deleted = false
            };

            _context.PasswordkLinks.Add(link);

            Utils.EmailFields details = new Utils.EmailFields
            {
                EmailTo = person.Email,
                EmailSubject = "Welcome to Intenr Hub",
                EmailBody = "Hello! \n\n" +
                           "We are glad to have you in our team as " + person.Status + ". You have to complete your account by seeting the password!\n\n" +
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
            var personToDelete = _mapper.Map<Person>(GetPersonById(personId));
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
            return _mapper.Map<IEnumerable<PersonDto>>(_context.People.ToList().Where(g => g.Deleted == false));
        }

        public PersonDto GetPersonById(int id)
        {
            return GetAllPerson().SingleOrDefault(p => p.PersonId == id);
        }

        public PersonDto GetPersonByUsername(string username)
        {
            return GetAllPerson().SingleOrDefault(p => p.Username == username);
        }

        public PersonDto LogIn(string email, string password)
        {
            var loggedPerson = _mapper.Map<Person>(GetPersonByEmail(email));
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
            var personFromDb = GetPersonById(person.PersonId);
            if (person.FirstName == null) person.FirstName = personFromDb.FirstName;
            if (person.LastName == null) person.LastName = personFromDb.LastName;
            if (person.Username == null) person.Username = personFromDb.Username;
            if (person.Email == null) person.Email = personFromDb.Email;
            if (person.BirthDate == null) person.BirthDate = personFromDb.BirthDate;
            if (person.Status == null) person.Status = personFromDb.Status;
            if (person.Password == null)
            {
                person.PasswordHash = personFromDb.PasswordHash;
                person.PasswordSalt = personFromDb.PasswordSalt;
            }
            else
            {
                using var hmac = new HMACSHA512();
                person.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(person.Password));
                person.PasswordSalt = hmac.Key;
            }
            _context.People.Update(_mapper.Map<Person>(person));
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
            return GetAllPerson().SingleOrDefault(p => p.Email == email);
        }
    }
}
