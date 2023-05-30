using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GroupChat
{
    public int GroupChatId { get; set; }

    public string GroupChatName { get; set; }

    public string GroupChatDescription { get; set; }

    public int AdminId { get; set; }

    public bool Deleted { get; set; }

    public virtual Person Admin { get; set; }

    public virtual ICollection<GroupChatMessage> GroupChatMessages { get; } = new List<GroupChatMessage>();

    public virtual ICollection<GroupChatPerson> GroupChatPeople { get; } = new List<GroupChatPerson>();
}
