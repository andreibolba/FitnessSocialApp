namespace API.Dtos
{
    public class TestDto
    {
        public int TestId { get; set; }
        public int TrainerId { get; set; }
        
        public string TestName { get; set; }

        public PersonDto Trainer { get; set; }

        public DateTime DateOfPost { get; set; }

        public DateTime? Deadline { get; set; }

        public bool? CanBeEdited { get; set; }

        public IEnumerable<QuestionDto> Questions { get; set;}
    }
}