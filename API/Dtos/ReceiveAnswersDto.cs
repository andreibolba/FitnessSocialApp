using API.Models;

namespace API.Dtos
{
    public class Answer
    {
        public int QuestionId { get; set; }
        public QuestionDto Question { get; set; }
        public string InternOption { get; set; }
    }
    public class ReceiveAnswersDto
    {
        public int InternId { get; set; }
        public int TestId { get; set; }
        public List<Answer> Answers { get; set;}
    }
}
