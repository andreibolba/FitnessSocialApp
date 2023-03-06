using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public sealed class InternGroupRepository : IInternGroupRepository
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
            var result = _mapper.Map<InternGroup>(GetInternGroupsById(id));
            result.Deleted = true;
            _context.InternGroups.Update(result);
        }

        public IEnumerable<InternGroupDto> GetAllInternGroups()
        {
            return _mapper.Map<List<InternGroupDto>>(_context.InternGroups.Where(g => g.Deleted == false).Include(g => g.Intern));
        }

        public IEnumerable<InternGroupDto> GetInternFromGroup(int groupId)
        {
            var result = GetAllInternGroups().Where(ig => ig.GroupId == groupId);
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
            return _mapper.Map<InternGroupDto>(GetAllInternGroups().SingleOrDefault(ig => ig.InternGroupId == id));
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public bool UpdateAllInternsInGroup(string ids, int groupId)
        {
            var result = _context.InternGroups.Where(g => g.Deleted == false && g.GroupId == groupId);
            List<int> idList = Utils.Utils.FromStringToInt(ids);
            if(result.Count()==0&& idList.Count()==0)
            return false;
            foreach (var res in result)
            {
                bool delete = idList.IndexOf(res.InternId) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(res.InternId);
                _context.InternGroups.Update(res);
            }

            foreach (var id in idList)
            {
                _context.InternGroups.Add(new InternGroup
                {
                    GroupId = groupId,
                    InternId = id,
                    Deleted = false
                });
            }
            return true;
        }
    }
}
