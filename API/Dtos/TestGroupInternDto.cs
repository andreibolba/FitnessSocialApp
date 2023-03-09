using API.Models;

namespace API.Dtos
{
    public class TestGroupInternDto
    {
        public int TestGroupId { get; set; }

        public int TestId { get; set; }

        public int? GroupId { get; set; }

        public int? InternId { get; set; }

        public Group Group { get; set; }

        public Person Intern { get; set; }

        public Test Test { get; set; }
        public bool IsDelete { get; set; }
    }
}
