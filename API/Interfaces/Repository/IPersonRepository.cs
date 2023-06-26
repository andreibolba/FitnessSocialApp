using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IPersonRepository
    {
        public PersonDto Create(PersonDto person);
        public IEnumerable<PersonDto> GetAllPerson();
        public IEnumerable<PersonDto> GetAllAdmins();
        public IEnumerable<PersonDto> GetAllInterns();
        public IEnumerable<PersonDto> GetAllTrainers();
        public PersonDto GetPersonById(int id);
        public Task<PersonDto> GetPersonByIdAsync(int id);
        public PersonDto GetPersonByUsername(string username);
        public PersonDto GetPersonByEmail(string email);
        public PersonDto Update(PersonDto person);
        public void Delete(int personId);
        public PersonDto LogIn(string email, string password);
        public bool SaveAll();
        public IEnumerable<TestDto> GetAllInternTests(int personId);
    }
}