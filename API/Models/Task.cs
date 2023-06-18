using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public int TrainerId { get; set; }

    public DateTime DateOfPost { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<SubTask> SubTasks { get; } = new List<SubTask>();

    public virtual ICollection<TaskInternGroup> TaskInternGroups { get; } = new List<TaskInternGroup>();

    public virtual ICollection<TaskSolution> TaskSolutions { get; } = new List<TaskSolution>();

    public virtual Person Trainer { get; set; }
}
