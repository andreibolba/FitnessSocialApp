using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace API.Data
{
    public sealed class TestRepository : ITestRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TestRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public TestDto Create(TestDto test)
        {
            var testToAdd = _mapper.Map<Test>(test);
            _context.Tests.Add(testToAdd);
            if (SaveAll() == true)
                return _mapper.Map<TestDto>(testToAdd);
            return new TestDto();
        }

        public void Delete(int testId)
        {
            var test = _mapper.Map<Test>(this.GetTestById(testId));
            test.Deleted = true;
            test.CanBeEdited = false;
            _context.Tests.Update(test);
            var allTestQuestion = _context.TestQuestions.Where(tq => tq.Deleted == false && tq.TestId == testId);
            foreach (var tq in allTestQuestion)
            {
                tq.Deleted = true;
                _context.TestQuestions.Update(tq);
            }
        }

        public IEnumerable<TestDto> GetAllTests()
        {
            var result = _mapper.Map<IEnumerable<TestDto>>(_context.Tests.Where(t => t.Deleted == false).Include(t => t.Trainer));
            foreach (var res in result)
            {
                res.Questions = _mapper.Map<IEnumerable<QuestionDto>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId == res.TestId).Select(t => t.Question));
            }
            return result;
        }

        public IEnumerable<TestDto> GetInternTest(int internId)
        {
            var allInternTests = _context.TestGroupInterns.Where(t => t.Deleted == false && t.InternId == internId).Select(t => t.TestId).ToList();
            List<TestDto> allTests = new List<TestDto>();
            var allInternGroups = _context.InternGroups.Where(t => t.Deleted == false && t.InternId == internId).Include(t => t.Group).Select(t => t.Group.TestGroupInterns);
            foreach (var t in allInternGroups)
                foreach (var r in t)
                {
                    if (allInternTests.Where(g => g == r.TestId).Count()==0)
                        allInternTests.Add(r.TestId);
                }
            foreach (var t in allInternTests)
                allTests.Add(GetTestById(t));
            return allTests;
        }

        public TestDto GetTestById(int id)
        {
            return this.GetAllTests().SingleOrDefault(t => t.TestId == id);
        }

        public IEnumerable<TestDto> GetTestByTrainerId(int trainerId)
        {
            return this.GetAllTests().Where(t => t.TrainerId == trainerId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void StopEdit(int id)
        {
            var test = _mapper.Map<Test>(this.GetTestById(id));
            test.CanBeEdited = false;
            _context.Tests.Update(test);

            var questions = _mapper.Map<IEnumerable<Question>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId == id).Select(t => t.Question));
            foreach (var q in questions)
            {
                q.CanBeEdited = false;
                _context.Questions.Update(q);
            }
        }

        public void Update(TestDto test)
        {
            var testUpdate = _mapper.Map<Test>(GetTestById(test.TestId));
            testUpdate.TestName = test.TestName == null ? testUpdate.TestName : test.TestName;
            testUpdate.Deadline = test.Deadline == null ? testUpdate.Deadline : test.Deadline.Value;
            testUpdate.CanBeEdited = test.CanBeEdited == null ? testUpdate.CanBeEdited : test.CanBeEdited.Value;
            testUpdate.TrainerId = test.Trainer == null ? testUpdate.TrainerId : test.Trainer.PersonId;

            _context.Tests.Update(testUpdate);
        }
    }
}
