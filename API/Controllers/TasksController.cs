using API.Data;
using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Authorize]
    public class TasksController : BaseAPIController
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInternGroupRepository _taskInternGroupRepository;
        private readonly ITaskSolutionRepository _taskSolutionRepository;

        public TasksController(ITaskRepository taskRepository, ITaskInternGroupRepository taskInternGroupRepository, ITaskSolutionRepository taskSolutionRepository)
        {
            _taskRepository = taskRepository;
            _taskInternGroupRepository = taskInternGroupRepository;
            _taskSolutionRepository = taskSolutionRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskDto>> GetAllTasks()
        {
            return Ok(_taskRepository.GetAllTasks());
        }

        [HttpGet("{taskId:int}")]
        public ActionResult<IEnumerable<TaskDto>> GetTask(int taskId)
        {
            return Ok(_taskRepository.GetTaskById(taskId));
        }

        [HttpGet("checked/group/{taskId:int}")]
        public ActionResult<IEnumerable<TaskGroupDto>> GetGroupsChecked(int taskId)
        {
            return Ok(_taskRepository.GetAllGroupsChecked(taskId));
        }

        [HttpGet("checked/intern/{taskId:int}")]
        public ActionResult<IEnumerable<TaskInternDto>> GetInternshecked(int taskId)
        {
            return Ok(_taskRepository.GetAllInternsChecked(taskId));
        }

        [HttpGet("{status}/{userId:int}")]
        public ActionResult<IEnumerable<TaskDto>> GetAllTasksForUser(string status, int userId)
        {
            switch (status)
            {
                case "trainer":
                    return Ok(_taskRepository.GetAllTasksForTrainer(userId));
                case "intern":
                    return Ok(_taskInternGroupRepository.GetAllTasksForStudent(userId));
                case "group":
                    return Ok(_taskInternGroupRepository.GetAllTasksForGroup(userId));
                default:
                    return BadRequest("Status not recognized!");
            }
        }

        [HttpPost("add")]
        public ActionResult<TaskDto> AddTask([FromBody] TaskDto task)
        {
            var res = _taskRepository.AddTask(task);
            return res != null ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("edit")]
        public ActionResult<TaskDto> EditTask([FromBody] TaskDto task)
        {
            var res = _taskRepository.UpdateTask(task);
            return res != null ? Ok(res) : BadRequest("Internal Server error");
        }

        [HttpPost("delete/{taskId:int}")]
        public ActionResult<TaskDto> DeleteTask(int taskId)
        {
            _taskRepository.DeleteTask(taskId);
            return _taskRepository.SaveAll() ? Ok() : BadRequest("Internal Server error");
        }

        [HttpPost("addedit/solution/{taskSolutionId:int}/{taskId:int}/{personId:int}")]
        public ActionResult<TaskSolutionDto> UpdateSolutionTask(int taskSolutionId, int taskId, int personId)
        {
            IFormFile file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
                return BadRequest("No file received!");

            long fileSize = file.Length;

            if (fileSize > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    byte[] zipFile = stream.ToArray();

                    if (taskSolutionId != -1)
                    {
                        var challengeSolution = _taskSolutionRepository.GetTaskSolutionById(taskSolutionId);
                        challengeSolution.SolutionFile = zipFile;

                        var res = _taskSolutionRepository.UpdateTaskSolution(challengeSolution);

                        return res != null ? Ok(res) : BadRequest("Internal Server Error");
                    }
                    else
                    {

                        var challengeSolution = new TaskSolutionDto();
                        challengeSolution.TaskId = taskId;
                        challengeSolution.InternId = personId;
                        challengeSolution.SolutionFile = zipFile;

                        var res = _taskSolutionRepository.AddTaskSolution(challengeSolution);

                        return res != null ? Ok(res) : BadRequest("Internal Server Error");
                    }
                }
            }
            return BadRequest("Internal Server Error");
        }

        [HttpPost("assign/{taskid:int}")]
        public ActionResult Assigntask([FromBody] object ids, int taskId)
        {
            Dictionary<string, string> idsData = JsonConvert.DeserializeObject<Dictionary<string, string>>(ids.ToString());

            var hasSomethingToSaveIntern = _taskInternGroupRepository.AssignTaskToStudents(idsData["idsIntern"] += "!", taskId);
            var hasSomethingToSaveGroup = _taskInternGroupRepository.AssignTaskToGroups(idsData["idsGroups"] += "!", taskId);
            if (!hasSomethingToSaveIntern && !hasSomethingToSaveGroup)
                return Ok();
            return _taskInternGroupRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }

        [HttpGet("solutions/{taskId:int}")]
        public ActionResult GetAllChallengeSolutinsForTask(int taskId)
        {
            return Ok(_taskSolutionRepository.GetAllSolutionsForTask(taskId));
        }

        [HttpGet("solutions/{taskId:int}/{personId:int}")]
        public ActionResult GetAllChallengeSolutinsForTaskForPerson(int taskId, int personId)
        {
            return Ok(_taskSolutionRepository.GetAllSolutionsForTaskForAPerson(taskId,personId));
        }

        [HttpGet("solutions/download/{taskSolutionId:int}")]
        public ActionResult GetSolutionFile(int taskSolutionId)
        {
            var sol = _taskSolutionRepository.GetTaskSolutionById(taskSolutionId);

            string contentType = "application/octet-stream";
            string fileName = "attempt_" + sol.Intern.Username + ".zip";

            return File(sol.SolutionFile, contentType, fileName);

        }
    }
}
