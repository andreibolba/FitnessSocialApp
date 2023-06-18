using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SubTasksController : BaseAPIController
    {
        private readonly ISubTaskRepository _subTaskRepository;

        public SubTasksController(ISubTaskRepository subTaskRepository)
        {
            _subTaskRepository = subTaskRepository;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<SubTaskDto>> GetAllSubTasks()
        {
            return Ok(_subTaskRepository.GetAllSubTasks());
        }

        [HttpGet("{taskId:int}")]
        public ActionResult<IEnumerable<SubTaskDto>> GetSubTaskById(int taskId)
        {
            return Ok(_subTaskRepository.GetAllSubTasksForATask(taskId));
        }

        [HttpPost("add")]
        public ActionResult<SubTaskDto> AddSubTask([FromBody] SubTaskDto task)
        {
            var res = _subTaskRepository.AddSubTask(task);
            return res != null ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("edit")]
        public ActionResult<SubTaskDto> EditSubTask([FromBody] SubTaskDto task)
        {
            var res = _subTaskRepository.UpdateSubTask(task);
            return res != null ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("delete/{subTaskId:int}")]
        public ActionResult<TaskDto> DeleteSubTask(int subTaskId)
        {
            _subTaskRepository.DeleteSubTask(subTaskId);
            return _subTaskRepository.SaveAll() ? Ok() : BadRequest("Internal Server error");
        }

        [HttpPost("check/{subTaskId:int}/{internId:int}")]
        public ActionResult<TaskSolutionDto> CheckSubTask(int subTaskId, int internId)
        {
            _subTaskRepository.CheckSubTask(subTaskId, internId);
            return _subTaskRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
