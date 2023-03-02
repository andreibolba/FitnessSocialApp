using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class GroupRepository : IGroupRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public GroupRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Create(GroupDto groupdto)
        {
            Group newGroup = new Group
            {
                GroupName = groupdto.GroupName,
                TrainerId = groupdto.TrainerId,
                Deleted = false
            };
            _context.Groups.Add(newGroup);
        }

        public void Delete(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(p => p.GroupId == groupId && p.Deleted == false);
            group.Deleted = true;
            _context.Groups.Update(group);

            var internGroups = _context.InternGroups.Where(ig => ig.GroupId == groupId);
            foreach (var ig in internGroups)
            {
                ig.Deleted = true;
                _context.InternGroups.Update(ig);
            }
        }

        public IEnumerable<GroupDto> GetAllGroups()
        {
            var groups = _context.Groups.Include(g => g.Trainer).Include(g => g.InternGroups).ToList().Where(g => g.Deleted == false);
            var groupsToReturn = _mapper.Map<List<GroupDto>>(groups);
            foreach (var re in groupsToReturn)
            {
                var interns = _context.InternGroups.Include(gi => gi.Intern).ToList().Where(gi => gi.Deleted == false && gi.GroupId == re.GroupId);
                re.AllInterns = _mapper.Map<List<PersonDto>>(interns);
            }
            return groupsToReturn;
        }

        public GroupDto GetGroupById(int id)
        {
            var group = _context.Groups.Include(g => g.Trainer).Include(g => g.InternGroups).SingleOrDefault(g => g.Deleted == false);
            var groupToReturn = _mapper.Map<GroupDto>(group);
            return groupToReturn;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(GroupDto groupdto)
        {
            Group groupToUpdate = new Group();
            groupToUpdate.GroupId = groupdto.GroupId;
            groupToUpdate.GroupName = groupdto.GroupName;
            groupToUpdate.TrainerId = groupdto.TrainerId;
            _context.Groups.Update(groupToUpdate);
        }
    }
}
