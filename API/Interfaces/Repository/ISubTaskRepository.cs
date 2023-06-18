using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ISubTaskRepository
    {
        public SubTaskDto AddSubTask(SubTaskDto subTask);
        public SubTaskDto UpdateSubTask(SubTaskDto subTask);
        public void DeleteSubTask(int  subTaskId);
        public SubTaskDto GetSubTask(int subTaskId);
        public IEnumerable<SubTaskDto> GetAllSubTasks();
        public IEnumerable<SubTaskDto> GetAllSubTasksForATask(int taskId);
        public bool SaveAll();
        public void CheckSubTask(int subTaskId, int internId);
    }
}
