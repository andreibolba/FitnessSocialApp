using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public FeedbackRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public FeedbackDto AddFeedback(FeedbackDto feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.DateOfPost = DateTime.Now;
            _context.Feedbacks.Add(feedback);

            return SaveAll() ? _mapper.Map<FeedbackDto>(feedback) : null;
        }

        public void DeleteFeedback(int id)
        {
            var feebdack = _mapper.Map<Feedback>(GetFeedbackById(id));
            feebdack.Deleted = true;

            _context.Feedbacks.Update(feebdack);
        }

        public FeedbackDto EditFeedback(FeedbackDto feedbackDto)
        {
            var feebdack = _mapper.Map<Feedback>(GetFeedbackById(feedbackDto.FeedbackId));

            feebdack.Content = feedbackDto.Content==null ? feebdack.Content : feedbackDto.Content;
            feebdack.ChallangeId = feedbackDto.ChallangeId;
            feebdack.TaskId = feedbackDto.TaskId;
            feebdack.TestId = feedbackDto.TestId;

            _context.Feedbacks.Update(feebdack);

            return SaveAll()? _mapper.Map<FeedbackDto>(feebdack) : null;
        }

        public IEnumerable<FeedbackDto> GetAllFeedback()
        {
            var res = _context.Feedbacks.Where(f => f.Deleted == false);
            return _mapper.Map<IEnumerable<FeedbackDto>>(res);
        }

        public IEnumerable<FeedbackDto> GetAllFeedbackForSpecificPerson(int personId)
        {
            return GetAllFeedback().Where(f=>f.InternId==personId);
        }

        public FeedbackDto GetFeedbackById(int id)
        {
            return GetAllFeedback().SingleOrDefault(f=>f.FeedbackId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }
    }
}
