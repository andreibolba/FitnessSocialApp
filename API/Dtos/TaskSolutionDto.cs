using API.Models;

namespace API.Dtos
{
    public class TaskSolutionDto
    {
        public int TaskSolutionId { get; set; }

        public int TaskId { get; set; }

        public int InternId { get; set; }

        public string SolutionContent { get; set; }

        public DateTime DateOfSolution { get; set; }

        public PersonDto Intern { get; set; }

        public TaskDto Task { get; set; }
    }
}
