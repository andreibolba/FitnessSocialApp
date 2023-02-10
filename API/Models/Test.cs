using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Test
{
    public int TestId { get; set; }

    public string TestName { get; set; }

    public int TrainerId { get; set; }

    public DateTime DateOfPost { get; set; }

    public DateTime Deadline { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; } = new List<Feedback>();

    public virtual ICollection<Question> Questions { get; } = new List<Question>();

    public virtual Person Trainer { get; set; }
}
