using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IQuestionRepository
    {
        QuestionDto Create(QuestionDto questionDto);
        IEnumerable<QuestionDto> GetAllQuestions();
        IEnumerable<QuestionDto> GetAllQuestionsByTrainerId(int trainerId);
        QuestionDto GetQuestionById(int id);
        QuestionDto Update(QuestionDto questionDto);
        void Delete(int questionid);
        void StopEdit(int id);
        bool SaveAll();
    }
}
