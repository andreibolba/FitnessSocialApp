using API.Models;

namespace API.Interfaces.Repository
{
    public interface IPasswordLinkRepository
    {
        void Create(PasswordkLink link);
        IEnumerable<PasswordkLink> GetAll();
        PasswordkLink GetById(int id);
        PasswordkLink Delete(PasswordkLink link);
        bool SaveAll();
    }
}
