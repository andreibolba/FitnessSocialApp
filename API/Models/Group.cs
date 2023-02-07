using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string GroupDescription { get; set; } = null!;

    public int Admin { get; set; }

    public bool Deleted { get; set; }

    public virtual Person AdminNavigation { get; set; } = null!;

    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
