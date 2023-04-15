namespace API.Dtos
{
    public class PostDto
    {
        public int? PostId { get; set; }

        public int? PersonId { get; set; }

        public PersonDto Person { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        public bool Upvote { get; set; }
        public bool Downvote { get; set; }

        public DateTime? DateOfPost { get; set; }

        public int Karma { get; set; }
        public int Views { get; set; }
    }
}
