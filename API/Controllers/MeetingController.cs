using API.Interfaces.Repository;

namespace API.Controllers
{
    public class MeetingController : BaseAPIController
    {
        private readonly IMeetingRepository _repository;

        public MeetingController(IMeetingRepository repository)
        {
            _repository = repository;
        }


    }
}
