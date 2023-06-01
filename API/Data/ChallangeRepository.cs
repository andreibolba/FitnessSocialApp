using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ChallangeRepository : IChallangeRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public ChallangeRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ChallangeDto CreateChallange(ChallangeDto challange)
        {
            var challangeToDb = _mapper.Map<Challange>(challange);

            challangeToDb.DateOfPost = DateTime.Now;

            _context.Challanges.Add(challangeToDb);

            return SaveAll() ? _mapper.Map<ChallangeDto>(challangeToDb) : null;
        }

        public void DeleteChallange(int id)
        {
            var challange = _mapper.Map<Challange>(GetChallangeById(id));
            challange.Deleted= true;
            foreach(var s in _context.ChallangeSolutions.Where(d=>d.Deleted==false&& d.ChallangeId == id))
            {
                s.Deleted= true;
                _context.ChallangeSolutions.Update(s);
            }
        }

        public IEnumerable<ChallangeDto> GetAllChallanges()
        {
            var res = _context.Challanges.Where(d => d.Deleted == false).Include(g => g.Trainer);
            return _mapper.Map<IEnumerable<ChallangeDto>>(res);
        }

        public IEnumerable<ChallangeDto> GetAllChallangesForSpecificDay(DateTime time)
        {
            return GetAllChallanges().Where(c=>c.DateOfPost.Value.Day == time.Day && c.DateOfPost.Value.Month == time.Month && c.DateOfPost.Value.Year == time.Year);
        }

        public ChallangeDto GetChallangeById(int id)
        {
            return GetAllChallanges().FirstOrDefault(c => c.ChallangeId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public ChallangeDto UpdateChallange(ChallangeDto challange)
        {
            var challangeToUpdate = _mapper.Map<Challange>(GetChallangeById(challange.ChallangeId));

            challangeToUpdate.ChallangeName = challange.ChallangeName==null? challangeToUpdate.ChallangeName : challange.ChallangeName;
            challangeToUpdate.ChallangeDescription = challange.ChallangeDescription == null? challangeToUpdate.ChallangeDescription : challange.ChallangeDescription;
            challangeToUpdate.Deadline = challange.Deadline == null? challangeToUpdate.Deadline : challange.Deadline;

            _context.Challanges.Update(challangeToUpdate);

            return SaveAll() ? _mapper.Map<ChallangeDto>(challangeToUpdate) : null;
        }
    }
}
