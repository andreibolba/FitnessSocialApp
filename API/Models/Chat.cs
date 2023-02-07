using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Chat
{
    public int ChatId { get; set; }

    public int PersonSenderId { get; set; }

    public int PersonReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime SendDate { get; set; }

    public bool Deleted { get; set; }

    public virtual Person PersonReceiver { get; set; } = null!;

    public virtual Person PersonSender { get; set; } = null!;
}
