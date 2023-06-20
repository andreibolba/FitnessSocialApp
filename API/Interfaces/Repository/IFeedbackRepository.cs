using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IFeedbackRepository
    {
        public IEnumerable<FeedbackDto> GetAllFeedback();
        public IEnumerable<FeedbackDto> GetAllFeedbackForSender(int senderId, int? count = null);
        public IEnumerable<FeedbackDto> GetAllFeedbackForReceiver(int receiverId, int? count = null);
        public FeedbackDto GetFeedbackById(int id);
        public FeedbackDto AddFeedback(FeedbackDto feedbackDto);
        public FeedbackDto EditFeedback(FeedbackDto feedbackDto);
        public void DeleteFeedback(int id);
        public bool SaveAll();

    }
}
