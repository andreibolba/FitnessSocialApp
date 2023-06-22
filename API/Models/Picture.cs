using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Picture
{
    public int PictureId { get; set; }

    public string Url { get; set; }

    public string PublicId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Chat> Chats { get; } = new List<Chat>();

    public virtual ICollection<GroupChatMessage> GroupChatMessages { get; } = new List<GroupChatMessage>();

    public virtual ICollection<GroupChat> GroupChats { get; } = new List<GroupChat>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<Person> People { get; } = new List<Person>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
