using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMeetingRepository
    {
        MeetingDto Create(MeetingDto meeting);
        IEnumerable<MeetingDto> GetAll();
        IEnumerable<MeetingDto> GetFutureMeetings();
        IEnumerable<MeetingDto> GetAllByTrainerId(int trainerId, int? count = null);
        MeetingDto GetMeetingById(int id);
        void Update(MeetingDto meeting);
        void Delete(int id);
        bool SaveAll();
    }
}
