using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IInternGroupRepository
    {
        void Create(InternGroup internGroup);
        IEnumerable<InternGroupDto> GetAllInternGroups();
        InternGroupDto GetInternGroupsById(int id);
        IEnumerable<InternGroupDto> GetInternFromGroup(int groupId);
        void Update(InternGroup internGroup);
        void UpdateAllInternsInGroup(string obj,int groupId);
        void Delete(int id);
        bool SaveAll();
    }
}
