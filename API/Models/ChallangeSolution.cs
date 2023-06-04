using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ChallangeSolution
{
    public int ChallangeSolutionId { get; set; }

    public int ChallangeId { get; set; }

    public DateTime DateOfSolution { get; set; }

    public int InternId { get; set; }

    public byte[] SolutionFile { get; set; }

    public int Points { get; set; }

    public bool? Approved { get; set; }

    public bool Deleted { get; set; }

    public virtual Challange Challange { get; set; }

    public virtual Person Intern { get; set; }
}
