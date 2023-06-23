using System.Security.Cryptography;
using System.Text;
using API.Dtos;
using API.Interfaces;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public sealed class PersonRepository : IPersonRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _token;
        private readonly IPictureRepository _pictureRepository;

        public PersonRepository(InternShipAppSystemContext context, IMapper mapper, ITokenService token, IPictureRepository pictureRepository)
        {
            _context = context;
            _mapper = mapper;
            _token = token;
            _pictureRepository = pictureRepository;
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

            if (this.SaveAll()==false)
            {
                return false;
            }

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
            var allPerson = _mapper.Map<IEnumerable<PersonDto>>(_context.People.ToList().Where(g => g.Deleted == false));
            foreach (var pers in allPerson)
            {
                var allReactionsPosts = _context.PostCommentReactions.Where(r => r.Deleted == false && r.PostId!=null).Include(r => r.Post).ToList();
                var allReactionsComments = _context.PostCommentReactions.Where(r => r.Deleted == false && r.CommentId !=null).Include(r => r.Comment).ToList();
                var picture = pers.PictureId !=null ? _pictureRepository.GetById(pers.PictureId.Value): null;

                var likesPost = allReactionsPosts.Where(p =>
                p.Post.PersonId == pers.PersonId
                && p.Upvote == true
                && p.DownVote == false).Count();
                var likesComment = allReactionsComments.Where(p =>
                p.Comment.PersonId == pers.PersonId
                && p.Upvote == true
                && p.DownVote == false).Count();

                var dislikesPost = allReactionsPosts.Where(p =>
                p.Post.PersonId == pers.PersonId
                && p.Upvote == false
                && p.DownVote == true).Count();
                var dislikesComment = allReactionsComments.Where(p =>
                p.Comment.PersonId == pers.PersonId
                && p.Upvote == false
                && p.DownVote == true).Count();

                pers.Picture = picture;
                pers.Karma = (likesComment+ likesPost) - (dislikesComment+dislikesPost);
                pers.Answers = _context.Comments.Where(c => c.Deleted == false && c.PersonId == pers.PersonId).Count();
            }
            return allPerson;
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
            if (person.BirthDate == null) person.BirthDate = personFromDb.BirthDate.Value;
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
            return this.GetAllPerson().Where(g => g.Status == "Intern");
        }

        public IEnumerable<PersonDto> GetAllTrainers()
        {
            return this.GetAllPerson().Where(g => g.Status == "Trainer");
        }

        public PersonDto GetPersonByEmail(string email)
        {
            return GetAllPerson().SingleOrDefault(p => p.Email == email);
        }

        public IEnumerable<TestDto> GetAllInternTests(int personId)
        {
            var result = _context.TestGroupInterns.Where(tgi => tgi.Deleted == false && (tgi.InternId != null && tgi.InternId == personId))
                .Include(tgi => tgi.Test)
                .Select(tgi => tgi.Test.TestId);
            var resultReturn = _mapper.Map<IEnumerable<TestDto>>(_context.Tests.Where(t => t.Deleted == false  && result.Contains(t.TestId)).Include(t => t.Trainer));
            foreach (var res in resultReturn)
            {
                res.Questions = _mapper.Map<IEnumerable<QuestionDto>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId == res.TestId).Select(t => t.Question));
            }
            return resultReturn;
        }

        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            var allPeople =  _mapper.Map<IEnumerable<PersonDto>>(_context.People.Where(p => p.Deleted == false));
            var ppl = await _context.People.Include(p=>p.Picture).SingleOrDefaultAsync(p=>p.Deleted == false && p.PersonId == id);
            return _mapper.Map<PersonDto>(ppl);
        }
    }
}
