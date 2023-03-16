using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMeetingRepository
    {
        MeetingDto Create(MeetingDto meeting);
        IEnumerable<MeetingDto> GetAll();
        IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId);
        IEnumerable<MeetingDto> GetAllByGroupId(int groupId);
        IEnumerable<MeetingDto> GetAllByInternId(int internId);
        MeetingDto GetMeetingById(int id);
        void Update(MeetingDto meeting);
        void Delete(int id);
        bool SaveAll();
    }
}
