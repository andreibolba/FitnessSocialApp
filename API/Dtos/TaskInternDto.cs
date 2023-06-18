namespace API.Dtos
{
    public class TaskInternDto
    {
        public int TaskId { get; set; }
        public int InternId { get; set; }
        public PersonDto Intern { get; set; }
        public bool IsChecked { get; set; }
    }
}
