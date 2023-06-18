using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;

namespace API.Data
{
    public class SubTaskRepository : ISubTaskRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public SubTaskRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public SubTaskDto AddSubTask(SubTaskDto subTask)
        {
            var task = _mapper.Map<SubTask>(subTask);
            _context.SubTasks.Add(task);
            return SaveAll() ? _mapper.Map<SubTaskDto>(task) : null;
        }

        public void CheckSubTask(int subTaskId, int internId)
        {
            var r = _context.SubTaskCheckeds.SingleOrDefault(s=>s.SubTaskId==subTaskId && s.InternId ==internId);
            if (r!=null)
            {
                r.Deleted=!r.Deleted;
                _context.SubTaskCheckeds.Update(r);
                return;
            }
            var res = new SubTaskChecked()
            {
                SubTaskId = subTaskId,
                InternId = internId,
                Deleted = false
            };
            _context.SubTaskCheckeds.Add(res);
        }

        public void DeleteSubTask(int subTaskId)
        {
            var res = _mapper.Map<SubTask>(GetSubTask(subTaskId));
            res.Deleted = true;
            _context.Update(res);
        }

        public IEnumerable<SubTaskDto> GetAllSubTasks()
        {
            var res = _context.SubTasks.Where(s => s.Deleted == false);
            return _mapper.Map<IEnumerable<SubTaskDto>>(res);
        }

        public IEnumerable<SubTaskDto> GetAllSubTasksForATask(int taskId)
        {
            return GetAllSubTasks().Where(s => s.TaskId == taskId);
        }

        public SubTaskDto GetSubTask(int subTaskId)
        {
            return GetAllSubTasks().SingleOrDefault(s => s.TaskId == subTaskId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public SubTaskDto UpdateSubTask(SubTaskDto subTask)
        {
            var task = _mapper.Map<SubTask>(GetSubTask(subTask.SubTaskId));

            task.SubTaskName = subTask.SubTaskName == null ? task.SubTaskName : subTask.SubTaskName;
            
            _context.SubTasks.Update(task);

            return SaveAll() ? _mapper.Map<SubTaskDto>(task) : null;
        }
    }
}
