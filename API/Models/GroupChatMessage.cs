using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GroupChatMessage
{
    public int GroupChatMessageId { get; set; }

    public int PersonId { get; set; }

    public int GroupChatId { get; set; }

    public string Message { get; set; }

    public DateTime SendDate { get; set; }

    public bool Deleted { get; set; }

    public virtual GroupChat GroupChat { get; set; }

    public virtual Person Person { get; set; }
}
