using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace API.Data
{
    public class TaskInternGroupRepository : ITaskInternGroupRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TaskInternGroupRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool AssignTaskToGroups(string ids, int taskId)
        {
            var result = _context.TaskInternGroups.Where(g => g.Deleted == false && g.GroupId != null && g.InternId == null && g.TaskId == taskId).ToList();
            List<int> idList = Utils.Utils.FromStringToInt(ids);
            if (result.Count() == 0 && idList.Count() == 0)
                return false;
            foreach (var res in result)
            {
                bool delete = idList.IndexOf(res.GroupId.Value) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(res.GroupId.Value);
                _context.TaskInternGroups.Update(res);
            }

            foreach (var id in idList)
            {
                _context.TaskInternGroups.Add(new TaskInternGroup
                {
                    GroupId = id,
                    TaskId = taskId,
                    InternId = null,
                    Deleted = false
                });
            }
            return true;
        }

        public bool AssignTaskToStudents(string ids, int taskId)
        {
            var result = _context.TaskInternGroups.Where(g => g.Deleted == false && g.GroupId == null && g.InternId != null && g.TaskId == taskId).ToList();
            List<int> idList = Utils.Utils.FromStringToInt(ids);
            if (result.Count() == 0 && idList.Count() == 0)
                return false;
            foreach (var res in result)
            {
                bool delete = idList.IndexOf(res.InternId.Value) == -1 ? true : false;
                res.Deleted = delete;
                if (delete == false)
                    idList.Remove(res.InternId.Value);
                _context.TaskInternGroups.Update(res);
            }

            foreach (var id in idList)
            {
                _context.TaskInternGroups.Add(new TaskInternGroup
                {
                    GroupId = null,
                    TaskId = taskId,
                    InternId = id,
                    Deleted = false
                });
            }
            return true;
        }

        public IEnumerable<TaskDto> GetAllTasksForGroup(int groupId)
        {
            var res = _context.TaskInternGroups.Where(g => g.GroupId == groupId && g.Deleted==false).Include(g => g.Task).Select(g => g.Task);
            var groups = _mapper.Map<IEnumerable<TaskDto>>(res);

            return groups.OrderBy(g=>g.DateOfPost);
        }

        public IEnumerable<TaskDto> GetAllTasksForStudent(int studentId)
        {
            var resStudents = _context.TaskInternGroups.Where(g => g.InternId == studentId && g.Deleted == false).Include(g => g.Task).Select(g => g.Task);
            var tasks = _mapper.Map<List<TaskDto>>(resStudents);

            var groupsId = _context.InternGroups.Where(g => g.InternId == studentId && g.Deleted == false).Select(g => g.GroupId);

            foreach(var id in groupsId)
                foreach(var task in GetAllTasksForGroup(id))
                    tasks.Add(task);

            return tasks.OrderBy(g=>g.DateOfPost);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
