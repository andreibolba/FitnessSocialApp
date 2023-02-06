using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public int PersonId { get; set; }

    public int ExerciseId { get; set; }

    public int Stars { get; set; }

    public string? Comment { get; set; }

    public bool Deleted { get; set; }

    public virtual Exercise Exercise { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
