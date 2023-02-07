using System;
using System.Collections.Generic;

namespace API.Models;

public partial class DaysOfWeek
{
    public int DaysOfWeekId { get; set; }

    public string DayName { get; set; } = null!;

    public bool Deleted { get; set; }

    public virtual ICollection<ExercisePlanning> ExercisePlannings { get; } = new List<ExercisePlanning>();
}
