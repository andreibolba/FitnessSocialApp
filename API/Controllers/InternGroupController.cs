using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers
{
    [Authorize]
    public class InternGroupController : BaseAPIController
    {
        private readonly IInternGroupRepository _repository;

        public InternGroupController(IInternGroupRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("interns/{id:int}")]
        public ActionResult<IEnumerable<InternGroupDto>> GetInternFromGroup(int id)
        {
            return Ok(_repository.GetInternFromGroup(id));
        }

        [HttpPost("interns/update/{groupId:int}")]
        public ActionResult EditInternIntoGroups([FromBody] object ids, int groupId)
        {
            Dictionary<string, string> idsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ids.ToString());

            var res = _repository.UpdateAllInternsInGroup(idsData["ids"] += "!", groupId);
            return res!=null ? Ok(res) : BadRequest("Internal Server Error");
        }
    }
}