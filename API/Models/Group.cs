using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public int? TrainerId { get; set; }

    public string Description { get; set; }

    public int? PictureId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<InternGroup> InternGroups { get; } = new List<InternGroup>();

    public virtual ICollection<MeetingInternGroup> MeetingInternGroups { get; } = new List<MeetingInternGroup>();

    public virtual Picture Picture { get; set; }

    public virtual ICollection<TaskInternGroup> TaskInternGroups { get; } = new List<TaskInternGroup>();

    public virtual ICollection<TestGroupIntern> TestGroupInterns { get; } = new List<TestGroupIntern>();

    public virtual Person Trainer { get; set; }
}
