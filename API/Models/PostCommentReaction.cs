using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostCommentReaction
{
    public int PostCommentLikeId { get; set; }

    public int PersonId { get; set; }

    public int? PostId { get; set; }

    public int? CommentId { get; set; }

    public bool Upvote { get; set; }

    public bool DoenVote { get; set; }

    public bool Deleted { get; set; }

    public virtual Comment Comment { get; set; }

    public virtual Person Person { get; set; }

    public virtual Post Post { get; set; }
}
