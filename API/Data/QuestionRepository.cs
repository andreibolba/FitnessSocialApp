using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public QuestionRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(QuestionDto questionDto)
        {
            _context.Questions.Add(_mapper.Map<Question>(questionDto));
        }

        public void Delete(int questionid)
        {
            var question = _mapper.Map<Question>(this.GetQuestionById(questionid));
            question.Deleted = true;
            question.CanBeEdited = false;

            var allQuestiontests=_context.TestQuestions.Where(q=>q.Deleted==false&&q.QuestionId==questionid);
            foreach(var q in allQuestiontests){
                q.Deleted=true;
                _context.TestQuestions.Update(q);
            }

            _context.Questions.Update(question);
        }

        public IEnumerable<QuestionDto> GetAllQuestions()
        {
            return _mapper.Map<IEnumerable<QuestionDto>>(_context.Questions.Where(q => q.Deleted == false).Include(q=>q.Trainer));
        }

        public IEnumerable<QuestionDto> GetAllQuestionsByTrainerId(int trainerId)
        {
            return this.GetAllQuestions().Where(q => q.TrainerId == trainerId);
        }

        public QuestionDto GetQuestionById(int id)
        {
            return this.GetAllQuestions().SingleOrDefault(q => q.QuestionId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void StopEdit(int id)
        {
            var question = _mapper.Map<Question>(this.GetQuestionById(id));
            question.CanBeEdited = false;
            _context.Questions.Update(question);
        }

        public void Update(QuestionDto questionDto)
        {
            var question = _mapper.Map<Question>(this.GetQuestionById(questionDto.QuestionId));
            question.QuestionName = questionDto.QuestionName == null ? question.QuestionName : questionDto.QuestionName;
            question.A = questionDto.A == null ? question.A : questionDto.A;
            question.B = questionDto.B == null ? question.B : questionDto.B;
            question.C = questionDto.C == null ? question.C : questionDto.C;
            question.D = questionDto.D == null ? question.D : questionDto.D;
            question.E = questionDto.E == null ? question.E : questionDto.E;
            question.F = questionDto.F == null ? question.F : questionDto.F;
            question.CorrectOption = questionDto.CorrectOption == null ? question.CorrectOption : questionDto.CorrectOption;
            question.Points = questionDto.Points == null ? question.Points : questionDto.Points.Value;
            question.TrainerId = questionDto.TrainerId == null ? question.TrainerId : questionDto.TrainerId.Value;

            _context.Questions.Update(question);
        }
    }
}
