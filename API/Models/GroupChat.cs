using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GroupChat
{
    public int GroupChatId { get; set; }

    public string GroupChatName { get; set; } = null!;

    public int Admin { get; set; }

    public bool Deleted { get; set; }

    public virtual Person AdminNavigation { get; set; } = null!;

    public virtual ICollection<GroupChatMessage> GroupChatMessages { get; } = new List<GroupChatMessage>();

    public virtual ICollection<GroupChatPerson> GroupChatPeople { get; } = new List<GroupChatPerson>();
}
