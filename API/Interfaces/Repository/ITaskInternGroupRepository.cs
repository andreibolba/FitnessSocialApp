using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ITaskInternGroupRepository
    {
        public IEnumerable<TaskDto> GetAllTasksForStudent(int studentId);
        public IEnumerable<TaskDto> GetAllTasksForGroup(int groupId);
        public bool AssignTaskToStudents(string ids, int taskId);
        public bool AssignTaskToGroups(string ids, int taskId);
        public bool SaveAll();
    }
}
