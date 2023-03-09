using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Question
{
    public int QuestionId { get; set; }

    public string QuestionName { get; set; }

    public int? TrainerId { get; set; }

    public string A { get; set; }

    public string B { get; set; }

    public string C { get; set; }

    public string D { get; set; }

    public string E { get; set; }

    public string F { get; set; }

    public string CorrectOption { get; set; }

    public int Points { get; set; }

    public bool CanBeEdited { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<QuestionSolution> QuestionSolutions { get; } = new List<QuestionSolution>();

    public virtual ICollection<TestQuestion> TestQuestions { get; } = new List<TestQuestion>();

    public virtual Person Trainer { get; set; }
}
