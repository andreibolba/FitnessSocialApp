using API.Dtos;
using API.Interfaces.Repository;
using API.Models;

namespace API.Data
{
    public class QuestionSolutionRepository : IQuestionSolutionRepository
    {
        private readonly InternShipAppSystemContext _context;

        public QuestionSolutionRepository(InternShipAppSystemContext context)
        {
            _context = context;
        }

        public void Create(ReceiveAnswersDto answers)
        {
            foreach (var ans in answers.Answers)
                _context.QuestionSolutions.Add(new QuestionSolution { 
                    InternId = answers.InternId, 
                    QuestionId = ans.QuestionId, 
                    InternOption = ans.InternOption, 
                    Deleted = false });
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
