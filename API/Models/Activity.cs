using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Activity
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public int AddedBy { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<ActivityRecord> ActivityRecords { get; } = new List<ActivityRecord>();

    public virtual Person AddedByNavigation { get; set; } = null!;
}
