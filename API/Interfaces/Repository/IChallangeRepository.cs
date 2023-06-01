using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IChallangeRepository
    {
        public ChallangeDto CreateChallange(ChallangeDto challange);
        public ChallangeDto UpdateChallange(ChallangeDto challange);
        public void DeleteChallange(int id);
        public ChallangeDto GetChallangeById(int id);
        public IEnumerable<ChallangeDto> GetAllChallanges();
        public IEnumerable<ChallangeDto> GetAllChallangesForSpecificDay(DateTime time);
        public bool SaveAll();
    }
}
