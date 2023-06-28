using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            task.DateOfPost = DateTime.Now;
            _context.Tasks.Add(task);
            return SaveAll() ? _mapper.Map<TaskDto>(task) : null;
        }

        public void DeleteTask(int taskId)
        {
            var res = _mapper.Map<Models.Task>(GetTaskById(taskId));
            res.Deleted = true;
            _context.Tasks.Update(res);

            foreach(var subTask in _context.SubTasks.Where(t=>t.TaskId ==taskId)) { 
                subTask.Deleted = true;
                _context.SubTasks.Update(subTask);
            }
        }

        public IEnumerable<TaskGroupDto> GetAllGroupsChecked(int taskId)
        {
            var groups = _context.TaskInternGroups.Where(g=>g.Deleted==false && g.TaskId == taskId && g.InternId==null).Include(f => f.Group).ToList();

            var groupsCheck = new List<TaskGroupDto>();

            foreach(var group in groups)
            {
                groupsCheck.Add(new TaskGroupDto()
                {
                    GroupId = group.GroupId.Value,
                    Group = _mapper.Map<GroupDto>(group.Group),
                    IsChecked = true
                });
            }

            var allGroups = _context.Groups.Where(g => g.Deleted == false).ToList();

            foreach (var group in allGroups)
            { 
                if(groupsCheck.Where(g=>g.GroupId == group.GroupId).Count()==0)
                    groupsCheck.Add(new TaskGroupDto()
                    {
                        GroupId = group.GroupId,
                        Group = _mapper.Map<GroupDto>(group),
                        IsChecked = false
                    });
            }

            return groupsCheck;
        }

        public IEnumerable<TaskInternDto> GetAllInternsChecked(int taskId)
        {
            var interns = _context.TaskInternGroups.Where(g => g.Deleted == false && g.TaskId == taskId && g.GroupId == null).Include(f=>f.Intern).ToList();

            var internsCheck = new List<TaskInternDto>();

            foreach (var intern in interns)
            {
                internsCheck.Add(new TaskInternDto()
                {
                    InternId = intern.InternId.Value,
                    Intern = _mapper.Map<PersonDto>(intern.Intern),
                    IsChecked = true
                });
            }

            var allInterns = _context.People.Where(g => g.Deleted == false && g.Status=="Intern").ToList();

            foreach (var intern in allInterns)
            {
                if (internsCheck.Where(g => g.InternId == intern.PersonId).Count() == 0)
                    internsCheck.Add(new TaskInternDto()
                    {
                        InternId = intern.PersonId,
                        Intern = _mapper.Map<PersonDto>(intern),
                        IsChecked = false
                    });
            }

            return internsCheck;
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

        public IEnumerable<TaskDto> GetAllTasksForGroup(int groupId)
        {
            var res = _context.TaskInternGroups.Where(d=>d.Deleted == false && d.GroupId == groupId && d.InternId==null).Select(g=>g.Task).AsEnumerable();
            return _mapper.Map<IEnumerable<TaskDto>>(res);
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
