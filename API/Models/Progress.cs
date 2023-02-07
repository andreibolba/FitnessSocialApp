using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Progress
{
    public int ProgressId { get; set; }

    public int PersonId { get; set; }

    public int BodyScore { get; set; }

    public decimal Weight { get; set; }

    public decimal Water { get; set; }

    public decimal BodyFat { get; set; }

    public decimal VisceralFat { get; set; }

    public decimal Bmi { get; set; }

    public decimal Muscle { get; set; }

    public decimal Protein { get; set; }

    public int BasalMetabolism { get; set; }

    public decimal BoneMass { get; set; }

    public int BodyAge { get; set; }

    public decimal IdealWeight { get; set; }

    public string Verdict { get; set; } = null!;

    public DateTime DateRegister { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
