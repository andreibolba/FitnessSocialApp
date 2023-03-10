using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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

        public void AddQuestionToTest(int testId,int questionId)
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

        public void AddTestToGroup(int testId, int groupId)
        {
            var exists = _context.TestGroupInterns.SingleOrDefault(tgi => tgi.TestId == testId && tgi.GroupId == groupId);
            if (exists != null)
            {
                exists.Deleted = false;
                _context.TestGroupInterns.Update(exists);
            }
            else
            {
                _context.TestGroupInterns.Add(new TestGroupIntern
                {
                    TestId = testId,
                    GroupId = groupId,
                    Deleted = false
                });
            }
        }

        public void AddTestToStudent(int testId, int internId)
        {
            var exists = _context.TestGroupInterns.SingleOrDefault(tgi => tgi.TestId == testId && tgi.InternId == internId);
            if (exists != null)
            {
                exists.Deleted = false;
                _context.TestGroupInterns.Update(exists);
            }
            else
            {
                _context.TestGroupInterns.Add(new TestGroupIntern
                {
                    TestId = testId,
                    InternId = internId,
                    Deleted = false
                });
            }
        }

        public void Create(TestDto test)
        {
            _context.Tests.Add(_mapper.Map<Test>(test));
        }

        public void Delete(int testId)
        {
            var test = _mapper.Map<Test>(this.GetTestById(testId));
            test.Deleted = true;
            test.CanBeEdited = false;
            _context.Tests.Update(test);
            var allTestQuestion = _context.TestQuestions.Where(tq => tq.Deleted == false && tq.TestId == testId);
            foreach(var tq in allTestQuestion)
            {
                tq.Deleted = true;
                _context.TestQuestions.Update(tq);
            }
        }

        public IEnumerable<TestDto> GetAllTests()
        {
            var result = _mapper.Map<IEnumerable<TestDto>>(_context.Tests.Where(t => t.Deleted == false).Include(t=>t.Trainer));
            foreach (var res in result)
            {
                res.Questions = _mapper.Map<IEnumerable<QuestionDto>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId==res.TestId).Select(t=>t.Question));
            }
            return result;
        }

        public IEnumerable<QuestionDto> GettAllQuestionsFromTest(int testId)
        {
            return _mapper.Map<IEnumerable<QuestionDto>>(_context.TestQuestions.Where(t => t.Deleted == false && t.TestId == testId).Select(t => t.Question));
        }

        public TestDto GetTestById(int id)
        {
            return this.GetAllTests().SingleOrDefault(t => t.TestId == id);
        }

        public IEnumerable<TestDto> GetTestByTrainerIdId(int trainerId)
        {
            return this.GetAllTests().Where(t => t.TrainerId == trainerId);
        }

        public void RemoveQuestionFromTest(int testId, int questionId)
        {
            var add = _context.TestQuestions.SingleOrDefault(q => q.TestId == testId && q.QuestionId == questionId);
                add.Deleted = true;
                _context.TestQuestions.Update(add);
        }

        public void RemoveTestFromGruop(int testId, int groupId)
        {
            var exists = _context.TestGroupInterns.SingleOrDefault(tgi => tgi.TestId == testId && tgi.GroupId == groupId);
            exists.Deleted= true;
            _context.TestGroupInterns.Update(exists);
        }

        public void RemoveTestFromStudents(int testId, int internId)
        {
            var exists = _context.TestGroupInterns.SingleOrDefault(tgi => tgi.TestId == testId && tgi.InternId == internId);
            exists.Deleted = true;
            _context.TestGroupInterns.Update(exists);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }

        public void StopEdit(int id)
        {
            var test = _mapper.Map<Test>(this.GetTestById(id));
            test.CanBeEdited = false;
            _context.Tests.Update(test);

            var questions = _mapper.Map<IEnumerable<Question>>(this.GettAllQuestionsFromTest(id));
            foreach(var q in questions)
            {
                q.CanBeEdited = false;
                _context.Questions.Update(q);
            }
        }

        public void Update(TestDto test)
        {
            var testUpdate = _mapper.Map<Test>(GetTestById(test.TestId));
            testUpdate.TestName = test.TestName==null? testUpdate.TestName : test.TestName;
            testUpdate.Deadline = test.Deadline == null? testUpdate.Deadline : test.Deadline.Value;
            testUpdate.CanBeEdited = test.CanBeEdited == null? testUpdate.CanBeEdited : test.CanBeEdited.Value;
            testUpdate.TrainerId = test.Trainer == null? testUpdate.TrainerId : test.Trainer.PersonId;

            _context.Tests.Update(testUpdate);
        }
    }
}
