using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int PersonId { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime DateOfPost { get; set; }

    public int? PictureId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual Person Person { get; set; }

    public virtual Picture Picture { get; set; }

    public virtual ICollection<PostCommentReaction> PostCommentReactions { get; } = new List<PostCommentReaction>();

    public virtual ICollection<PostPicture> PostPictures { get; } = new List<PostPicture>();

    public virtual ICollection<PostView> PostViews { get; } = new List<PostView>();
}
