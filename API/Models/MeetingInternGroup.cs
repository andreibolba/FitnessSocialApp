using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MeetingInternGroup
{
    public int MeetingInternGroupId { get; set; }

    public int MeetingId { get; set; }

    public int? InternId { get; set; }

    public int? GroupId { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Meeting Meeting { get; set; }
}
