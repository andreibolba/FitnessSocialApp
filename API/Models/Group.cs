using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int TrainerId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<InternGroup> InternGroups { get; } = new List<InternGroup>();

    public virtual ICollection<Meeting> Meetings { get; } = new List<Meeting>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual Person Trainer { get; set; }
}
