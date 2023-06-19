using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        public IEnumerable<FeedbackDto> GetAllFeedback();
        public IEnumerable<FeedbackDto> GetAllFeedbackForSpecificPerson(int personId);
        public FeedbackDto GetFeedbackById(int id);
        public FeedbackDto AddFeedback(FeedbackDto feedbackDto);
        public FeedbackDto EditFeedback(FeedbackDto feedbackDto);
        public void DeleteFeedback(int id);
        public bool SaveAll();

    }
}
