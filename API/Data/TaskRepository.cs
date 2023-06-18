using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TaskDto AddTask(TaskDto taskDto)
        {
            var task = _mapper.Map<Models.Task>(taskDto);
            _context.Tasks.Add(task);
            return SaveAll() ? _mapper.Map<TaskDto>(task) : null;
        }

        public void DeleteTask(int taskId)
        {
            var res = _mapper.Map<Models.Task>(GetTaskById(taskId));
            res.Deleted = true;
            _context.Update(res);
        }

        public IEnumerable<TaskDto> GetAllTasks()
        {
            var res = _context.Tasks.Where(t => t.Deleted == false);
            return _mapper.Map<IEnumerable<TaskDto>>(res);
        }

        public IEnumerable<TaskDto> GetAllTasksForTrainer(int trainerId)
        {
            return GetAllTasks().Where(t=>t.TrainerId == trainerId);
        }

        public TaskDto GetTaskById(int taskId)
        {
            return GetAllTasks().SingleOrDefault(t => t.TaskId == taskId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public TaskDto UpdateTask(TaskDto taskDto)
        {
            var task = _mapper.Map<Models.Task>(GetTaskById(taskDto.TaskId));

            task.TaskName = taskDto.TaskName == null ? task.TaskName : taskDto.TaskName;
            task.TaskDescription = taskDto.TaskDescription == null ? task.TaskDescription : taskDto.TaskDescription;

            _context.Tasks.Update(task);

            return SaveAll() ? _mapper.Map<TaskDto>(task) : null;
        }
    }
}
