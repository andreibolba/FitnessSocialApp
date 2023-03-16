using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IMeetingRepository
    {
        void Create(MeetingDto meeting);
        IEnumerable<MeetingDto> GetAll();
        IEnumerable<MeetingDto> GetAllByPersonId(int personId);
        MeetingDto GetMeetingById(int id);
        void Update(MeetingDto meeting);
        void Delete(int id);
        bool SaveAll();
    }
}
