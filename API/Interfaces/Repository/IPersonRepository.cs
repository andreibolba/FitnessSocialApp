using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IPersonRepository
    {
        bool Create(PersonDto person);
        IEnumerable<PersonDto> GetAllPerson();
        IEnumerable<PersonDto> GetAllAdmins();
        IEnumerable<PersonDto> GetAllInterns();
        IEnumerable<PersonDto> GetAllTrainers();
        PersonDto GetPersonById(int id);
        PersonDto GetPersonByUsername(string username);
        PersonDto GetPersonByEmail(string email);
        void Update(PersonDto person);
        void Delete(int personId);
        PersonDto LogIn(string email, string password);
        bool SaveAll();
    }
}