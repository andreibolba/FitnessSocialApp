using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ChallangeSolution
{
    public int ChallangeSolutionId { get; set; }

    public int ChallangeId { get; set; }

    public DateTime DateOfSolution { get; set; }

    public int InternId { get; set; }

    public string SolutionLink { get; set; }

    public bool Deleted { get; set; }

    public virtual Challange Challange { get; set; }

    public virtual Person Intern { get; set; }
}
