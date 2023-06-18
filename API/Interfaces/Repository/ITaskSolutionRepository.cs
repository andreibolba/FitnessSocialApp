using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface ITaskSolutionRepository
    {
        public bool SaveAll();
        public IEnumerable<TaskSolutionDto> GetAllTaskSolutions();
        public IEnumerable<TaskSolutionDto> GetAllTaskSolutionsForTask(int taskId);
        public TaskSolutionDto GetTaskSolutionById(int taskSolutionId);
        public TaskSolutionDto AddTaskSolution(TaskSolutionDto taskSolutionDto);
        public TaskSolutionDto UpdateTaskSolution(TaskSolutionDto taskSolutionDto);
        public IEnumerable<TaskSolutionDto> GetAllSolutionsForTask(int taskId);
        public TaskSolutionDto GetAllSolutionsForTaskForAPerson(int taskId, int personId);
        public void DeleteTaskSolution(int taskSolutionId);
    }
}
