using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface IChallangeSolutionRpository
    {
        public ChallangeSolutionDto CreateSolution(ChallangeSolutionDto solution);
        public ChallangeSolutionDto UpdateSolution(ChallangeSolutionDto solution);
        public void DeleteSolution(int id);
        public ChallangeSolutionDto GetSolutionById(int id);
        public IEnumerable<ChallangeSolutionDto> GetAllSolutions();
        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForIntern(int internId);
        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForChallange(int challangeId);
        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForSpecificDay(DateTime time);
        public bool SaveAll();

    }
}
