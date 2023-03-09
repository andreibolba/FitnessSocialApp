using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TestQuestion
{
    public int TestQuestionId { get; set; }

    public int TestId { get; set; }

    public int QuestionId { get; set; }

    public bool Deleted { get; set; }

    public virtual Question Question { get; set; }

    public virtual Test Test { get; set; }
}
