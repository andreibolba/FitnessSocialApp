using System;
using System.Collections.Generic;

namespace API.Models;

public partial class SubTaskChecked
{
    public int SubTaskCheckedId { get; set; }

    public int SubTaskId { get; set; }

    public int InternId { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Intern { get; set; }

    public virtual SubTask SubTask { get; set; }
}
