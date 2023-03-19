using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class QuestionSolutionRepository : IQuestionSolutionRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public QuestionSolutionRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(ReceiveAnswersDto answers)
        {
            foreach (var ans in answers.Answers)
                _context.QuestionSolutions.Add(new QuestionSolution { 
                    InternId = answers.InternId, 
                    TestId=answers.TestId,
                    QuestionId = ans.QuestionId, 
                    InternOption = ans.InternOption, 
                    Deleted = false });
        }

        public IEnumerable<Answer> GetAllAnswers(int testId, int internId)
        {
            var allAnswers = _context.QuestionSolutions.Where(qs => qs.InternId == internId && qs.TestId == testId && qs.Deleted == false).Include(qs=>qs.Question).OrderBy(qs=>qs.QuestionId);
            return _mapper.Map<IEnumerable<Answer>>(allAnswers);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
