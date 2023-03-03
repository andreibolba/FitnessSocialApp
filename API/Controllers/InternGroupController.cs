using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Controllers
{
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
            return Ok(_repository.GetInternGroupsById(id));
        }

        [HttpPost("interns/update/{groupId:int}")]
        public ActionResult EditInternIntoGroups([FromBody] object ids, int groupId)
        {
            Dictionary<string, string> idsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ids.ToString());
            _repository.UpdateAllInternsInGroup(idsData["ids"] += "!", groupId);
            return _repository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}