using API.Interfaces.Repository;
using API.Models;

namespace API.Data
{
    public sealed class PasswordLinkRepository : IPasswordLinkRepository
    {
        private readonly InternShipAppSystemContext _context;

        public PasswordLinkRepository(InternShipAppSystemContext context)
        {
            _context = context;
        }

        public void Create(PasswordkLink link)
        {
            _context.PasswordkLinks.Add(link);
        }

        public PasswordkLink Delete(PasswordkLink link)
        {
            link.Deleted = true;
            _context.PasswordkLinks.Update(link);
            return SaveAll() ? link : null;
        }

        public IEnumerable<PasswordkLink> GetAll()
        {
            return _context.PasswordkLinks.ToList().Where(p=>p.Deleted==false);
        }

        public PasswordkLink GetById(int id)
        {
            return this.GetAll().SingleOrDefault(p => p.PasswordLinkId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
