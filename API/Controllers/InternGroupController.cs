using API.Dtos;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class InternGroupController : BaseAPIController
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public InternGroupController(InternShipAppSystemContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        [HttpGet("interns/{id:int}")]
        public ActionResult<IEnumerable<InternGroupDto>> GetInternFromGroup(int id)
        {
            var resultToReturn = _mapper.Map<List<InternGroupDto>>(_context.InternGroups.Where(g => g.Deleted == false && g.GroupId == id).Include(g => g.Intern));
            var resultUncheckedToReturn = _mapper.Map<IEnumerable<InternGroupDto>>( _context.People.Where(g => g.Deleted == false && g.Status=="Intern"));

            foreach (var re in resultUncheckedToReturn)
                if (resultToReturn.Any(r => r.PersonId == re.PersonId) == false)
                    resultToReturn.Add(re);
            return Ok(resultToReturn);
        }

        [HttpPost("interns/update/{groupId:int}")]
        public ActionResult EditInternIntoGroups([FromBody] object ids, int groupId)
        {
            Dictionary<string, string> idsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ids.ToString()); ;
            var result = _context.InternGroups.Where(g => g.Deleted == false && g.GroupId == groupId);
            List<int> idList = Utils.Utils.FromStringToInt(idsData["ids"] += "!");

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