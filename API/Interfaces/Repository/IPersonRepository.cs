using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IPersonRepository
    {
        void Create(Person person);
        IEnumerable<PersonDto> GetAllPerson();
        PersonDto GetPersonById(int id);
        PersonDto GetPersonByUsername(string username);
        void Update(Person person);
        void Delete(Person person);
        PersonDto Register(Person person);
        PersonDto LogIn(Person person);
        bool SaveAll();
    }
}