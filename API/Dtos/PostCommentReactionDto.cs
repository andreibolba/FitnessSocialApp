using API.Models;

namespace API.Dtos
{
    public class PostCommentReactionDto
    {
        public int? PostCommentLikeId { get; set; }

        public int? PersonId { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        public bool? Upvote { get; set; }

        public bool? Downvote { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual Person Person { get; set; }

        public virtual Post Post { get; set; }
    }
}
