namespace API.Dtos
{
    public class InternGroupDto
    {
        public int InternGroupId { get; set; }
        public int GroupId { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool IsChecked { get; set; }
    }
}