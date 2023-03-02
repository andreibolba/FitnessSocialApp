using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class GroupController : BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public GroupController(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetGroups()
        {
            var result = _context.Groups.Include(g => g.Trainer).Include(g=>g.InternGroups).ToList().Where(g => g.Deleted == false);
            var resultToReturn = _mapper.Map<IEnumerable<GroupDto>>(result);
            //foreach (var re in result)
            //{
            //    var allInterns = _context.InternGroups.Include(gi => gi.Intern).ToList().Where(gi => gi.Deleted == false && gi.GroupId == re.GroupId);

            //    List<PersonDto> interns = new List<PersonDto>();

            //    foreach (var i in allInterns)
            //    {
            //        interns.Add(new PersonDto()
            //        {
            //            PersonId = i.Intern.PersonId,
            //            FirstName = i.Intern.FirstName,
            //            LastName = i.Intern.LastName,
            //            Email = i.Intern.Email,
            //            Username = i.Intern.Username,
            //            Status = i.Intern.Status,
            //            BirthDate = i.Intern.BirthDate
            //        });
            //    }

            //    resultToReturn.Add(new GroupDto
            //    {
            //        GroupId = re.GroupId,
            //        Name = re.GroupName,
            //        Trainer = new PersonDto
            //        {
            //            PersonId = re.Trainer.PersonId,
            //            FirstName = re.Trainer.FirstName,
            //            LastName = re.Trainer.LastName,
            //            Email = re.Trainer.Email,
            //            Username = re.Trainer.Username,
            //            Status = re.Trainer.Status,
            //            BirthDate = re.Trainer.BirthDate
            //        },
            //        TrainerId = re.Trainer.PersonId,
            //        AllInterns = interns
            //    });
            //}
            return Ok(resultToReturn);
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            Group newGroup = new Group
            {
                GroupName = group.GroupName,
                TrainerId = group.TrainerId,
                Deleted = false
            };
            _context.Groups.Add(newGroup);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("delete/{groupId:int}")]
        public ActionResult DeleteGroup(int groupId)
        {
            var group = _context.Groups.FirstOrDefault(p => p.GroupId == groupId && p.Deleted == false);
            if (group == null)
                return BadRequest("Group is already deleted!");
            group.Deleted = true;
            _context.Groups.Update(group);

            var internGroups = _context.InternGroups.Where(ig => ig.GroupId == groupId);
            foreach (var ig in internGroups)
            {
                ig.Deleted = true;
                _context.InternGroups.Update(ig);
            }
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("update")]
        public ActionResult UpdateGroup([FromBody] GroupDto group)
        {
            var groupToUpdate = _context.Groups.FirstOrDefault(p => p.GroupId == group.GroupId && p.Deleted == false);
            if (groupToUpdate == null)
                return BadRequest("Group does not exists!");
            groupToUpdate.GroupName = group.GroupName;
            groupToUpdate.TrainerId = group.TrainerId;
            _context.Groups.Update(groupToUpdate);
            _context.SaveChanges();
            return Ok();
        }
    }
}