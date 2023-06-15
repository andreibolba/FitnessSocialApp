using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ChallengeSolutionRepository : IChallengeSolutionRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public ChallengeSolutionRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void ApproveDeclineSolutin(int solutionId, bool approved,int points)
        {
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(solutionId));

            sol.Points = points;
            sol.Approved = approved;

            _context.ChallangeSolutions.Update(sol);
        }

        public ChallengeSolutionDto CreateSolution(ChallengeSolutionDto solution)
        {
            var sol = _mapper.Map<ChallangeSolution>(solution);

            sol.DateOfSolution = DateTime.Now;
            sol.Approved = null;
            sol.Points = 0;
            
            _context.ChallangeSolutions.Add(sol);

            return SaveAll() ? _mapper.Map<ChallengeSolutionDto>(sol) : null;
        }

        public void DeleteSolution(int id)
        {
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(id)); 

            sol.Deleted = true;

            _context.ChallangeSolutions.Update(sol);    
        }

        public IEnumerable<ChallengeSolutionDto> GetAllSolutions()
        {
            var res = _context.ChallangeSolutions.Where(c => c.Deleted == false).Include(c=>c.Intern).Include(c=>c.Challange);
            return _mapper.Map<IEnumerable<ChallengeSolutionDto>>(res);
        }

        public IEnumerable<ChallengeSolutionDto> GetAllSolutionsForChallenge(int challangeId)
        {
            return GetAllSolutions().Where(c=>c.ChallangeId == challangeId);  
        }

        public IEnumerable<ChallengeSolutionDto> GetAllSolutionsForIntern(int internId)
        {
            return GetAllSolutions().Where(c => c.InternId == internId);
        }

        public IEnumerable<ChallengeSolutionDto> GetAllSolutionsForInternForChallenge(int internId, int challengeId)
        {
            return GetAllSolutionsForIntern(internId).Where(c => c.ChallangeId == challengeId);
        }

        public IEnumerable<ChallengeSolutionDto> GetAllSolutionsForSpecificDay(DateTime time)
        {
            return GetAllSolutions().Where(c => c.DateOfSolution.Day == time.Day && c.DateOfSolution.Month == time.Month && c.DateOfSolution.Year == time.Year);
        }

        public ChallengeSolutionDto GetSolutionById(int id)
        {
            return GetAllSolutions().FirstOrDefault(c => c.ChallangeSolutionId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }

        public ChallengeSolutionDto UpdateSolution(ChallengeSolutionDto solution)
        {
            var sol = _context.ChallangeSolutions.Where(c => c.InternId == solution.InternId 
            && c.ChallangeId == solution.ChallangeId 
            && c.Approved==null
            && c.Deleted==false);

            foreach(var s in sol)
            {
                s.Deleted = true;
                _context.ChallangeSolutions.Update(s);  
            }

            if (SaveAll() == false)
                return null;

            return CreateSolution(solution);
        }
    }
}
