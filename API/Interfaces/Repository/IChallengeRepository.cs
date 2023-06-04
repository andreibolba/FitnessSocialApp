using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IChallengeRepository
    {
        public ChallengeDto CreateChallenge(ChallengeDto challange);
        public ChallengeDto UpdateChallenge(ChallengeDto challange);
        public void DeleteChallenge(int id);
        public ChallengeDto GetChallengeById(int id);
        public IEnumerable<ChallengeDto> GetAllChallenges();
        public ChallengeDto GetAllChallengeForSpecificDay(DateTime time);
        public bool ExistsChallengeForSpecificDate(int year, int month, int day, int id = -1);
        public bool SaveAll();
    }
}
