using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Challange
{
    public int ChallangeId { get; set; }

    public string ChallangeName { get; set; }

    public string ChallangeDescription { get; set; }

    public int TrainerId { get; set; }

    public DateTime DateOfPost { get; set; }

    public DateTime Deadline { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<ChallangeSolution> ChallangeSolutions { get; } = new List<ChallangeSolution>();

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual Person Trainer { get; set; }
}
