using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int PersonId { get; set; }

    public int PostId { get; set; }

    public DateTime CommentContent { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual Post Post { get; set; } = null!;

    public virtual ICollection<PostCommentLike> PostCommentLikes { get; } = new List<PostCommentLike>();

    public virtual ICollection<PostPic> PostPics { get; } = new List<PostPic>();
}
