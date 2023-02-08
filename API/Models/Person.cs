using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime BirthDate { get; set; }

    public bool IsAdmin { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Activity> Activities { get; } = new List<Activity>();

    public virtual ICollection<ActivityRecord> ActivityRecords { get; } = new List<ActivityRecord>();

    public virtual ICollection<Chat> ChatPersonReceivers { get; } = new List<Chat>();

    public virtual ICollection<Chat> ChatPersonSenders { get; } = new List<Chat>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<ExercisePlanning> ExercisePlannings { get; } = new List<ExercisePlanning>();

    public virtual ICollection<Exercise> Exercises { get; } = new List<Exercise>();

    public virtual ICollection<Follow> FollowPersonFolloweds { get; } = new List<Follow>();

    public virtual ICollection<Follow> FollowPersonFollows { get; } = new List<Follow>();

    public virtual ICollection<GroupChatMessage> GroupChatMessages { get; } = new List<GroupChatMessage>();

    public virtual ICollection<GroupChatPerson> GroupChatPeople { get; } = new List<GroupChatPerson>();

    public virtual ICollection<GroupChat> GroupChats { get; } = new List<GroupChat>();

    public virtual ICollection<GroupPerson> GroupPeople { get; } = new List<GroupPerson>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<PersonPicture> PersonPictures { get; } = new List<PersonPicture>();

    public virtual ICollection<PostCommentLike> PostCommentLikes { get; } = new List<PostCommentLike>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    public virtual ICollection<Progress> Progresses { get; } = new List<Progress>();

    public virtual ICollection<Recipe> Recipes { get; } = new List<Recipe>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual ICollection<TableManagement> TableManagements { get; } = new List<TableManagement>();
}
