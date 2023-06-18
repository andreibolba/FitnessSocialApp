using API.Models;

namespace API.Dtos
{
    public class TaskInternGroupDto
    {
        public int TaskInternGroupId { get; set; }

        public int TaskId { get; set; }

        public int? InternId { get; set; }

        public int? GroupId { get; set; }

        public GroupDto Group { get; set; }

        public PersonDto Intern { get; set; }

        public TaskDto Task { get; set; }
    }
}
