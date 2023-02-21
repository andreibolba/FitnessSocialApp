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
        public ActionResult<IEnumerable<GroupDto>> GetPeople()
        {
            var result = _context.Groups.Include(g => g.Trainer).ToList().Where(g=>g.Deleted==false);
            var resultToReturn = new List<GroupDto>(0);
            foreach (var re in result)
            {
                resultToReturn.Add(new GroupDto
                {
                    GroupId=re.GroupId,
                    Name = re.GroupName,
                    Trainer = new LoggedPersonDto
                    {
                        PersonId= re.Trainer.PersonId,
                        FirstName = re.Trainer.FirstName,
                        LastName = re.Trainer.LastName,
                        Email = re.Trainer.Email,
                        Username = re.Trainer.Username,
                        Status = re.Trainer.Status,
                        BirthDate = re.Trainer.BirthDate
                    },
                    MembersCount = _context.InternGroups.Count(g=>g.GroupId==re.GroupId)
                });
            }
            return resultToReturn;
        }
    }
}