using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public sealed class GroupRepository : IGroupRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly ITestRepository _testRepository;
        private readonly IMapper _mapper;

        public GroupRepository(InternShipAppSystemContext context, ITestRepository testRepository, IMapper mapper)
        {
            _context = context;
            _testRepository = testRepository;
            _mapper = mapper;
        }

        public void Create(GroupDto groupdto)
        {
            _context.Groups.Add(_mapper.Map<Group>(groupdto));
        }
        
        public void Delete(int groupId)
        {
            var group = _mapper.Map<Group>(GetGroupById(groupId));
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

        public IEnumerable<TestDto> GetAllTestsFromGroup(int groupId)
        {
            var result = _context.TestGroupInterns
                .Where(tgi => tgi.Deleted == false && (tgi.GroupId != null && tgi.GroupId == groupId)).ToList();
            var allTests = _testRepository.GetAllTests();
            var resultTests = new List<TestDto>();
            foreach (var test in allTests)
                if (result.Count(t => t.TestId == test.TestId) != 0)
                    resultTests.Add(_testRepository.GetTestById(test.TestId));
            return resultTests;
        }

        public IEnumerable<PersonDto> GetAllParticipants(int groupId)
        {
            
            var result = _context.InternGroups
                .Where(ig => ig.Deleted == false && ig.GroupId == groupId )
                .Include(tgi => tgi.Intern)
                .Select(tgi => tgi.Intern)
                .ToList();
            var groupAdmin = _mapper.Map<Person>(GetGroupById(groupId).Trainer);
            result.Add(groupAdmin);
            return _mapper.Map<IEnumerable<PersonDto>>(result);
        }

        public GroupDto GetGroupById(int id)
        {
            return _mapper.Map<GroupDto>(GetAllGroups().SingleOrDefault(g=>g.GroupId==id));
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void Update(GroupDto groupdto)
        {
            var groypFromDb = GetGroupById(groupdto.GroupId);
            if (groupdto.GroupName == null) groupdto.GroupName = groypFromDb.GroupName;
            // if (groupdto.TrainerId == null) 
            groupdto.TrainerId = groypFromDb.TrainerId;
            _context.Groups.Update(_mapper.Map<Group>(groupdto));
        }
    }
}
