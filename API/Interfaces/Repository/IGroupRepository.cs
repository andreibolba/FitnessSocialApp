using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IGroupRepository
    {
        GroupDto Create(GroupDto groupdto);
        IEnumerable<GroupDto> GetAllGroups();
        IEnumerable<PersonDto> GetAllParticipants(int groupId);
        GroupDto GetGroupById(int id);
        GroupDto Update(GroupDto groupdto);
        void Delete(int groupId);
        bool SaveAll();
        IEnumerable<TestDto> GetAllTestsFromGroup(int groupId);
    }
}
