using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PostView
{
    public int PostViewId { get; set; }

    public int PersonId { get; set; }

    public int? PostId { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; }

    public virtual Post Post { get; set; }
}
