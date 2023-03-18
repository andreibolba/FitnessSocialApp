using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Meeting
{
    public int MeetingId { get; set; }

    public string MeetingName { get; set; }

    public string MeetingLink { get; set; }

    public int TrainerId { get; set; }

    public DateTime MeetingStartTime { get; set; }

    public DateTime MeetingFinishTime { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<MeetingInternGroup> MeetingInternGroups { get; } = new List<MeetingInternGroup>();

    public virtual Person Trainer { get; set; }
}
