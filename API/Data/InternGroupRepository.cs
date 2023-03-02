using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class InternGroupRepository : IInternGroupRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public InternGroupRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(InternGroup internGroup)
        {
            _context.InternGroups.Add(internGroup);
        }

        public void Delete(int id)
        {
            var result = _context.InternGroups.SingleOrDefault(g => g.Deleted == false);
            result.Deleted = true;
            _context.InternGroups.Update(result);
        }

        public IEnumerable<InternGroupDto> GetAllInternGroups()
        {
            var result = _context.InternGroups.Where(g => g.Deleted == false).Include(g => g.Intern);
            var resultToReturn = _mapper.Map<List<InternGroupDto>>(result);
            return resultToReturn;
        }

        public IEnumerable<InternGroupDto> GetInternFromGroup(int groupId)
        {
            var result = _context.InternGroups.Where(g => g.Deleted == false && g.GroupId == groupId).Include(g => g.Intern);
            var resultToReturn = _mapper.Map<List<InternGroupDto>>(result);
            var resultUnCheched = _context.People.Where(g => g.Deleted == false && g.Status == "Intern");
            var resultUncheckedToReturn = _mapper.Map<IEnumerable<InternGroupDto>>(resultUnCheched);

            foreach (var re in resultUncheckedToReturn)
                if (resultToReturn.Any(r => r.PersonId == re.PersonId) == false)
                    resultToReturn.Add(re);
            return resultToReturn;
        }

        public InternGroupDto GetInternGroupsById(int id)
        {
            var result = _context.InternGroups.Where(g => g.Deleted == false).Include(g => g.Intern);
            var resultToReturn = _mapper.Map<InternGroupDto>(result);
            return resultToReturn;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(InternGroup internGroup)
        {
            _context.InternGroups.Update(internGroup);
        }

        public void UpdateAllInternsInGroup(string ids, int groupId)
        {
            var result = _context.InternGroups.Where(g => g.Deleted == false && g.GroupId == groupId);
            List<int> idList = Utils.Utils.FromStringToInt(ids);

            foreach (var res in result)
            {
                bool delete = idList.IndexOf(res.InternId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(res.InternId);
            }

            foreach (var id in idList)
            {
                var gi = _context.InternGroups.FirstOrDefault(gi => gi.InternId == id && gi.GroupId == groupId);
                if (gi == null)
                    _context.InternGroups.Add(new InternGroup
                    {
                        GroupId = groupId,
                        InternId = id,
                        Deleted = false
                    });
                else
                {
                    gi.Deleted = false;
                    _context.InternGroups.Update(gi);
                }
            }

            _context.SaveChanges();
        }
    }
}
