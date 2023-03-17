using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMeetingRepository
    {
        MeetingDto Create(MeetingDto meeting);
        IEnumerable<MeetingDto> GetAll();
        IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count = null);
        IEnumerable<MeetingDto> GetAllByGroupId(int groupId, int? count = null);
        IEnumerable<MeetingDto> GetAllByInternId(int internId,int? count=null);
        MeetingDto GetMeetingById(int id);
        void Update(MeetingDto meeting);
        void Delete(int id);
        bool SaveAll();
    }
}
