namespace API.Dtos
{
    public class PostDto
    {
        public int? PostId { get; set; }

        public int? PersonId { get; set; }
        public PersonDto Person { get; set; }

        public string Content { get; set; }

        public DateTime? DateOfPost { get; set; }
    }
}
