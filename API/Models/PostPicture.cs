using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostPicture
{
    public int PostPicturesId { get; set; }

    public int PostId { get; set; }

    public string Picture { get; set; }

    public bool Deleted { get; set; }

    public virtual Post Post { get; set; }
}
