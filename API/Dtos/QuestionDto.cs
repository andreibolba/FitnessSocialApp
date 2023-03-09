using API.Models;

namespace API.Dtos
{
    public class QuestionDto
    {
        public int QuestionId { get; set; }
        public int? TrainerId { get; set; }
        public PersonDto Trainer { get; set; }

        public string QuestionName { get; set; }

        public int TestId { get; set; }

        public string A { get; set; }

        public string B { get; set; }

        public string C { get; set; }

        public string D { get; set; }

        public string E { get; set; }

        public string F { get; set; }

        public string CorrectOption { get; set; }

        public int? Points { get; set; }

        public bool? CanBeEdited { get; set; }

    }
}
