namespace API.Dtos
{
    public class MeetingDto
    {
        public int MeetingId { get; set; }

        public string MeetingName { get; set; }

        public string MeetingLink { get; set; }

        public int TrainerId { get; set; }

        public int? GroupId { get; set; }

        public int? InternId { get; set; }

        public DateTime MeetingStartTime { get; set; }

        public DateTime MeetingFinishTime { get; set; }

        public GroupDto Group { get; set; }

        public PersonDto Intern { get; set; }

        public PersonDto Trainer { get; set; }
    }
}
