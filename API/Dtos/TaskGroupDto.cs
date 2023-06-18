namespace API.Dtos
{
    public class TaskGroupDto
    {
        public int TaskId { get; set; }
        public int GroupId { get; set; }
        public GroupDto Group { get; set; }
        public bool IsChecked { get; set; }
    }
}
