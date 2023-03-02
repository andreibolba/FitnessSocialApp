using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IGroupRepository
    {
        void Create(Group group);
        IEnumerable<GroupDto> GetAllGroups();
        GroupDto GetGroupById(int id);
        void Update(Group group);
        void Delete(Group group);
        bool SaveAll();
    }
}
