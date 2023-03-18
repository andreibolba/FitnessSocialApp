namespace API.Dtos
{
    public class MeetingDto
    {
        public int MeetingId { get; set; }

        public string MeetingName { get; set; }

        public string MeetingLink { get; set; }

        public int TrainerId { get; set; }

        public DateTime MeetingStartTime { get; set; }

        public DateTime MeetingFinishTime { get; set; }

        public PersonDto Trainer { get; set; }
        public IEnumerable<PersonDto> AllPeopleInMeeting { get; set; }
    }
}
