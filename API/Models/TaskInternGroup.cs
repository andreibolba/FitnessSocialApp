using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TaskInternGroup
{
    public int TaskInternGroupId { get; set; }

    public int TaskId { get; set; }

    public int? InternId { get; set; }

    public int? GroupId { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Task Task { get; set; }
}
