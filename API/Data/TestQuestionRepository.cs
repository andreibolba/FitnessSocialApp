using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public sealed class TestQuestionRepository:ITestQuestionRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TestQuestionRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddQuestionToTest(int testId, int questionId)
        {
            var add = _context.TestQuestions.SingleOrDefault(q => q.TestId == testId && q.QuestionId == questionId);
            if (add != null)
            {
                add.Deleted = false;
                _context.TestQuestions.Update(add);
            }
            else
            {
                _context.TestQuestions.Add(new TestQuestion
                {
                    TestId = testId,
                    QuestionId = questionId,
                    Deleted = false
                });
            }
        }

        public IEnumerable<QuestionDto> GettAllQuestionsFromTest(int testId)
        {
            return _mapper.Map<IEnumerable<QuestionDto>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId == testId).Select(t => t.Question));
        }

        public IEnumerable<QuestionDto> GetUnselectedQuestions(int testId)
        {
            var allSelected = GettAllQuestionsFromTest(testId);
            var all = _mapper.Map<IEnumerable<QuestionDto>>(_context.Questions.Where(q => q.Deleted == false));
            var allUnseleted = new List<QuestionDto>();
            foreach (var a in all)
                if (allSelected.FirstOrDefault(q => q.QuestionId == a.QuestionId) == null)
                    allUnseleted.Add(a);
            return _mapper.Map<IEnumerable<QuestionDto>>(allUnseleted);
        }

        public void RemoveQuestionFromTest(int testId, int questionId)
        {
            var add = _context.TestQuestions.SingleOrDefault(q => q.TestId == testId && q.QuestionId == questionId);
            add.Deleted = true;
            _context.TestQuestions.Update(add);
        }

        public bool UpdateAllQuestions(string obj, int testId)
        {
            var result = _context.TestQuestions.Where(t => t.Deleted == false && t.TestId == testId);
            List<int> idList = Utils.Utils.FromStringToInt(obj);
            if (result.Count() == 0 && idList.Count() == 0)
                return false;
            foreach (var res in result)
            {
                bool delete = idList.IndexOf(res.QuestionId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(res.QuestionId);
                _context.TestQuestions.Update(res);
            }

            foreach (var id in idList)
            {
                _context.TestQuestions.Add(new TestQuestion
                {
                    TestId = testId,
                    QuestionId = id,
                    Deleted = false
                });
            }
            return true;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
