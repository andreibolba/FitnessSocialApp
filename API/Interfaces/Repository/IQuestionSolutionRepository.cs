using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IQuestionSolutionRepository
    {
        IEnumerable<Answer> GetAllAnswers(int testId,int internId);
        void Create(ReceiveAnswersDto answers);
        bool SaveAll();
    }
}
