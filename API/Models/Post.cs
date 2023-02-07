using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int PersonId { get; set; }

    public int? ProgressId { get; set; }

    public int? ExerciseId { get; set; }

    public int? ExercisePlanningId { get; set; }

    public string? Content { get; set; }

    public DateTime DateOfPost { get; set; }

    public int? GroupId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual Exercise? Exercise { get; set; }

    public virtual ExercisePlanning? ExercisePlanning { get; set; }

    public virtual Group? Group { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<PostCommentLike> PostCommentLikes { get; } = new List<PostCommentLike>();

    public virtual ICollection<PostPic> PostPics { get; } = new List<PostPic>();

    public virtual Progress? Progress { get; set; }
}
