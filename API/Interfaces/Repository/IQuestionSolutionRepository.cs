using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IQuestionSolutionRepository
    {
        void Create(ReceiveAnswersDto answers);
        bool SaveAll();
    }
}
