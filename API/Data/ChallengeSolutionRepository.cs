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

        public void ApproveDeclineSolutin(int solutionId, bool approved)
        {
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(solutionId));

            sol.Approved = approved;

            _context.ChallangeSolutions.Update(sol);
        }

        public ChallengeSolutionDto CreateSolution(ChallengeSolutionDto solution)
        {
            var sol = _mapper.Map<ChallangeSolution>(solution);

            sol.DateOfSolution = DateTime.Now;
            
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

        public IEnumerable<ChallengeSolutionDto> GetAllSolutionsForSpecificDay(DateTime time)
        {
            return GetAllSolutions().Where(c => c.DateOfSolution.Value.Day == time.Day && c.DateOfSolution.Value.Month == time.Month && c.DateOfSolution.Value.Year == time.Year);
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
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(solution.ChallangeSolutionId));

            sol.SolutionContent = solution.SolutionContent==null ? sol.SolutionContent : solution.SolutionContent;
            sol.SolutionFile = solution.SolutionFile == null ? sol.SolutionFile : solution.SolutionFile;
            sol.DateOfSolution = DateTime.Now;
            
            _context.ChallangeSolutions.Update(sol);

            return SaveAll() ? _mapper.Map<ChallengeSolutionDto>(sol) : null;
        }
    }
}
