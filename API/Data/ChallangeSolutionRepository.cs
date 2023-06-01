using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ChallangeSolutionRepository : IChallangeSolutionRpository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public ChallangeSolutionDto CreateSolution(ChallangeSolutionDto solution)
        {
            var sol = _mapper.Map<ChallangeSolution>(solution);

            sol.DateOfSolution = DateTime.Now;
            
            _context.ChallangeSolutions.Add(sol);

            return SaveAll() ? _mapper.Map<ChallangeSolutionDto>(sol) : null;
        }

        public void DeleteSolution(int id)
        {
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(id)); 

            sol.Deleted = true;

            _context.ChallangeSolutions.Update(sol);    
        }

        public IEnumerable<ChallangeSolutionDto> GetAllSolutions()
        {
            var res = _context.ChallangeSolutions.Where(c => c.Deleted == false).Include(c=>c.Intern).Include(c=>c.Challange);
            return _mapper.Map<IEnumerable<ChallangeSolutionDto>>(res);
        }

        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForChallange(int challangeId)
        {
            return GetAllSolutions().Where(c=>c.ChallangeId == challangeId);  
        }

        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForIntern(int internId)
        {
            return GetAllSolutions().Where(c => c.InternId == internId);
        }

        public IEnumerable<ChallangeSolutionDto> GetAllSolutionsForSpecificDay(DateTime time)
        {
            return GetAllSolutions().Where(c => c.DateOfSolution.Value.Day == time.Day && c.DateOfSolution.Value.Month == time.Month && c.DateOfSolution.Value.Year == time.Year);
        }

        public ChallangeSolutionDto GetSolutionById(int id)
        {
            return GetAllSolutions().FirstOrDefault(c => c.ChallangeSolutionId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }

        public ChallangeSolutionDto UpdateSolution(ChallangeSolutionDto solution)
        {
            var sol = _mapper.Map<ChallangeSolution>(GetSolutionById(solution.ChallangeSolutionId));

            sol.SolutionLink = solution.SolutionLink==null ? sol.SolutionLink : solution.SolutionLink;
            sol.DateOfSolution = DateTime.Now;
            
            _context.ChallangeSolutions.Update(sol);

            return SaveAll() ? _mapper.Map<ChallangeSolutionDto>(sol) : null;
        }
    }
}
