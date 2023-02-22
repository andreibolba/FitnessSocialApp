using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class GroupController : BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;

        public GroupController(InternShipAppSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GroupDto>> GetGroups()
        {
            var result = _context.Groups.Include(g => g.Trainer).ToList().Where(g => g.Deleted == false);
            var resultToReturn = new List<GroupDto>(0);
            foreach (var re in result)
            {
                resultToReturn.Add(new GroupDto
                {
                    GroupId = re.GroupId,
                    Name = re.GroupName,
                    Trainer = new LoggedPersonDto
                    {
                        PersonId = re.Trainer.PersonId,
                        FirstName = re.Trainer.FirstName,
                        LastName = re.Trainer.LastName,
                        Email = re.Trainer.Email,
                        Username = re.Trainer.Username,
                        Status = re.Trainer.Status,
                        BirthDate = re.Trainer.BirthDate
                    },
                    TrainerId=re.Trainer.PersonId,
                    MembersCount = _context.InternGroups.Where(gi => gi.Deleted == false).Count(g => g.GroupId == re.GroupId)
                });
            }
            return resultToReturn;
        }

        [HttpPost("add")]
        public ActionResult AddGroup([FromBody] GroupDto group)
        {
            Group newGroup = new Group
            {
                GroupName = group.Name,
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

            var internGroups=_context.InternGroups.Where(ig=>ig.GroupId==groupId);
            foreach(var ig in internGroups){
                ig.Deleted=true;
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
            groupToUpdate.GroupName = group.Name;
            groupToUpdate.TrainerId = group.TrainerId;
            _context.Groups.Update(groupToUpdate);
            _context.SaveChanges();
            return Ok();
        }
    }
}