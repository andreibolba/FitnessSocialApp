using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GroupChatPerson
{
    public int GroupChatUserId { get; set; }

    public int PersonId { get; set; }

    public int GroupChatId { get; set; }

    public bool Deleted { get; set; }

    public virtual GroupChat GroupChat { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
