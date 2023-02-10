using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int PersonId { get; set; }

    public int PostId { get; set; }

    public string CommentContent { get; set; }

    public DateTime DateOfComment { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; }

    public virtual Post Post { get; set; }

    public virtual ICollection<PostCommentReaction> PostCommentReactions { get; } = new List<PostCommentReaction>();
}
