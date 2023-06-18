using API.Models;

namespace API.Dtos
{
    public class SubTaskDto
    {
        public int SubTaskId { get; set; }

        public string SubTaskName { get; set; }

        public int TaskId { get; set; }

        public virtual TaskDto Task { get; set; }
    }
}
