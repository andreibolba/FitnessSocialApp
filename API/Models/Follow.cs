using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Follow
{
    public int FollowId { get; set; }

    public int PersonFollowId { get; set; }

    public int PersonFollowedId { get; set; }

    public bool Deleted { get; set; }

    public virtual Person PersonFollow { get; set; } = null!;

    public virtual Person PersonFollowed { get; set; } = null!;
}
