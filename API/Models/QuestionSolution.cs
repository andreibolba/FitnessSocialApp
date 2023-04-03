using System;
using System.Collections.Generic;

namespace API.Models;

public partial class QuestionSolution
{
    public int QuestionSolutionId { get; set; }

    public int TestId { get; set; }

    public int QuestionId { get; set; }

    public int InternId { get; set; }

    public string InternOption { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Question Question { get; set; }

    public virtual Test Test { get; set; }
}
