using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TaskSolution
{
    public int TaskSolutionId { get; set; }

    public int TaskId { get; set; }

    public int InternId { get; set; }

    public string SolutionLink { get; set; }

    public DateTime DateOfSolution { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Task Task { get; set; }
}
