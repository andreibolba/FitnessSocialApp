using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class TaskSolutionRepository : ITaskSolutionRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public TaskSolutionRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public TaskSolutionDto AddTaskSolution(TaskSolutionDto taskSolutionDto)
        {
            var taskSolution = _mapper.Map<TaskSolution>(taskSolutionDto);
            taskSolution.DateOfSolution = DateTime.Now;
            _context.TaskSolutions.Add(taskSolution);
            return SaveAll() ? _mapper.Map<TaskSolutionDto>(taskSolution) : null; ;
        }

        public void DeleteTaskSolution(int taskSolutionId)
        {
            var res = _mapper.Map<TaskSolution>(GetTaskSolutionById(taskSolutionId));
            res.Deleted = true;
            _context.Update(res);
        }

        public IEnumerable<TaskSolutionDto> GetAllTaskSolutions()
        {
            var res = _context.TaskSolutions.Where(t => t.Deleted == false);
            return _mapper.Map<IEnumerable<TaskSolutionDto>>(res);
        }

        public IEnumerable<TaskSolutionDto> GetAllTaskSolutionsForTask(int taskId)
        {
            return GetAllTaskSolutions().Where(t => t.TaskId == taskId);
        }


        public TaskSolutionDto GetTaskSolutionById(int taskSolutionId)
        {
            return GetAllTaskSolutions().SingleOrDefault(t => t.TaskSolutionId == taskSolutionId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public TaskSolutionDto UpdateTaskSolution(TaskSolutionDto taskSolutionDto)
        {
            var sol = _context.TaskSolutions.SingleOrDefault(s => s.TaskSolutionId == taskSolutionDto.TaskId);

            sol.DateOfSolution = DateTime.Now;
            sol.SolutionFile = taskSolutionDto.SolutionFile;

            _context.TaskSolutions.Update(sol);

            return SaveAll() ? _mapper.Map<TaskSolutionDto>(sol) : null;
        }

        public IEnumerable<TaskSolutionDto> GetAllSolutionsForTask(int taskId)
        {
            var res = _context.TaskSolutions.Where(t => t.Deleted == false && t.TaskId == taskId).Include(t=>t.Intern).ToList();
            return _mapper.Map<IEnumerable<TaskSolutionDto>>(res);
        }
        public TaskSolutionDto GetAllSolutionsForTaskForAPerson(int taskId, int personId)
        {
            var res = _context.TaskSolutions.SingleOrDefault(t => t.Deleted == false && t.TaskId == taskId && t.InternId==personId);
            return _mapper.Map<TaskSolutionDto>(res);
        }

    }
}
