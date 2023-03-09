using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IQuestionRepository
    {
        void Create(QuestionDto questionDto);
        IEnumerable<QuestionDto> GetAllQuestions();
        QuestionDto GetQuestionById(int id);
        void Update(QuestionDto questionDto);
        void Delete(int questionid);
        void StopEdit(int id);
        bool SaveAll();
    }
}
