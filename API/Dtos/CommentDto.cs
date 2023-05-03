using API.Models;

namespace API.Dtos
{
    public class CommentDto
    {
        public int? CommentId { get; set; }

        public int? PersonId { get; set; }

        public int? PostId { get; set; }

        public string CommentContent { get; set; }

        public DateTime? DateOfComment { get; set; }

        public int? Karma { get; set; }

        public bool Upvote { get; set; }

        public bool Downvote { get; set; }

        public PersonDto Person { get; set; }

        public PostDto Post { get; set; }
    }
}
