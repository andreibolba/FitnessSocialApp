using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ExercisePlanning
{
    public int ExercisePlanningId { get; set; }

    public int PlannerUserId { get; set; }

    public int DaysOfWeekId { get; set; }

    public int ExerciseId { get; set; }

    public bool Deleted { get; set; }

    public virtual DaysOfWeek DaysOfWeek { get; set; } = null!;

    public virtual Exercise Exercise { get; set; } = null!;

    public virtual Person PlannerUser { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
