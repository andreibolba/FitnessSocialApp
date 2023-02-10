using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Meeting
{
    public int MeetingId { get; set; }

    public string MeetingName { get; set; }

    public string MeetingLink { get; set; }

    public int TrainerId { get; set; }

    public int? GroupId { get; set; }

    public int? InternId { get; set; }

    public DateTime MeetingStartTime { get; set; }

    public DateTime MeetingFinishTime { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Person Trainer { get; set; }
}
