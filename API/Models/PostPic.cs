using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostPic
{
    public int PostPicId { get; set; }

    public int? PostId { get; set; }

    public int? CommentId { get; set; }

    public string Picture { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual Comment? Comment { get; set; }

    public virtual Post? Post { get; set; }
}
