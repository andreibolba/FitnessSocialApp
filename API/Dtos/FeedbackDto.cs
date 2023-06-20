using API.Models;

namespace API.Dtos
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }

        public int PersonSenderId { get; set; }

        public int PersonReceiverId { get; set; }

        public int? TaskId { get; set; }

        public int? ChallangeId { get; set; }

        public int? TestId { get; set; }

        public double? Grade { get; set; }

        public string Content { get; set; }

        public DateTime DateOfPost { get; set; }

        public ChallengeDto Challange { get; set; }

        public PersonDto PersonSender { get; set; }

        public TaskDto Task { get; set; }

        public TestDto Test { get; set; }

        public PersonDto PersonReceiver { get; set; }
    }
}
