using System;
using System.Collections.Generic;

namespace API.Models;

public partial class SubTask
{
    public int SubTaskId { get; set; }

    public string SubTaskName { get; set; }

    public int TaskId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<SubTaskChecked> SubTaskCheckeds { get; } = new List<SubTaskChecked>();

    public virtual Task Task { get; set; }
}
