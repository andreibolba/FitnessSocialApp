using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        public IEnumerable<FeedbackDto> GetAllFeedback();
        public IEnumerable<FeedbackDto> GetAllFeedbackForSpecificPerson(int personId, int? count = null);
        public IEnumerable<FeedbackDto> GetAllFeedbackForSpecificTrainer(int trainerId, int? count = null);
        public FeedbackDto GetFeedbackById(int id);
        public FeedbackDto AddFeedback(FeedbackDto feedbackDto);
        public FeedbackDto EditFeedback(FeedbackDto feedbackDto);
        public void DeleteFeedback(int id);
        public bool SaveAll();

    }
}
