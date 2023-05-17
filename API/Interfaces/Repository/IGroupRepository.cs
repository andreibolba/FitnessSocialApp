using API.Dtos;
using API.Models;

namespace API.Interfaces.Repository
{
    public interface IGroupRepository
    {
        void Create(GroupDto groupdto);
        IEnumerable<GroupDto> GetAllGroups();
        IEnumerable<PersonDto> GetAllParticipants(int groupId);
        GroupDto GetGroupById(int id);
        void Update(GroupDto groupdto);
        void Delete(int groupId);
        void SetPicture(int groupId, byte[] picture);
        bool SaveAll();
        IEnumerable<TestDto> GetAllTestsFromGroup(int groupId);
    }
}
