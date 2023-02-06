using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Exercise
{
    public int ExerciseId { get; set; }

    public int AddedBy { get; set; }

    public int MuscleGroupId { get; set; }

    public DateTime ExerciseName { get; set; }

    public bool ExerciseDesription { get; set; }

    public bool Deleted { get; set; }

    public virtual Person AddedByNavigation { get; set; } = null!;

    public virtual ICollection<ExercisePlanning> ExercisePlannings { get; } = new List<ExercisePlanning>();

    public virtual MuscleGroup MuscleGroup { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();
}
