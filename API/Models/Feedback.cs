﻿using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int PersonSenderId { get; set; }

    public int PersonReceiverId { get; set; }

    public int? TaskId { get; set; }

    public int? ChallangeId { get; set; }

    public int? TestId { get; set; }

    public string Content { get; set; }

    public double Grade { get; set; }

    public DateTime DateOfPost { get; set; }

    public bool Deleted { get; set; }

    public virtual Challange Challange { get; set; }

    public virtual Person PersonReceiver { get; set; }

    public virtual Person PersonSender { get; set; }

    public virtual Task Task { get; set; }

    public virtual Test Test { get; set; }
}
