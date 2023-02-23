using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class InternGroupController : BaseAPIController
    {
        private InternShipAppSystemContext _context;

        public InternGroupController(InternShipAppSystemContext context)
        {
            _context = context;
        }

        [HttpGet("interns/{id:int}")]
        public ActionResult<IEnumerable<InternGroupDto>> GetInternFromGroup(int id)
        {
            var result = _context.People.ToList().Where(g => g.Deleted == false && g.Status == "Intern");
            var resultToReturn = new List<InternGroupDto>(0);

            foreach (var re in result)
            {
                resultToReturn.Add(new InternGroupDto
                {
                    Intern = new LoggedPersonDto
                    {
                        PersonId = re.PersonId,
                        FirstName = re.FirstName,
                        LastName = re.LastName,
                        Email = re.Email,
                        Username = re.Username,
                        Status = re.Status,
                        BirthDate = re.BirthDate
                    },
                    InternId = re.PersonId,
                    IsChecked = _context.InternGroups.Any(ig => ig.Deleted == false && ig.InternId == re.PersonId && ig.GroupId == id)
                });
            }
            return resultToReturn;
        }

        [HttpPost("interns/update/{groupId:int}")]
        public ActionResult EditInternIntoGroups([FromBody] object ids, int groupId)
        {
            Dictionary<string, string> idsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ids.ToString());;
            var result = _context.InternGroups.Where(g => g.Deleted == false && g.GroupId == groupId);
            List<int> idList = Utils.Utils.FromStringToInt(idsData["ids"]+= "!");

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

            return Ok();
        }
    }
}