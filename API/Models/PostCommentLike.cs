using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostCommentLike
{
    public int PostCommentLikeId { get; set; }

    public int PersonId { get; set; }

    public int PostId { get; set; }

    public int CommentId { get; set; }

    public bool Deleted { get; set; }

    public virtual Comment Comment { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;
}
