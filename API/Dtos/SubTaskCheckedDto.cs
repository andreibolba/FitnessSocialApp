using API.Models;

namespace API.Dtos
{
    public class SubTaskCheckedDto
    {
        public int SubTaskCheckedId { get; set; }

        public int SubTaskId { get; set; }

        public int InternId { get; set; }

        public PersonDto Intern { get; set; }

        public SubTaskDto SubTask { get; set; }
    }
}
