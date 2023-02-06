using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MuscleGroup
{
    public int MuscleGroupId { get; set; }

    public int MuscleGroupName { get; set; }

    public int MuscleGroupDescription { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Exercise> Exercises { get; } = new List<Exercise>();
}
