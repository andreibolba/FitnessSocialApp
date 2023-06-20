using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string Username { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public string Status { get; set; }

    public DateTime BirthDate { get; set; }

    public int? PictureId { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<ChallangeSolution> ChallangeSolutions { get; } = new List<ChallangeSolution>();

    public virtual ICollection<Challange> Challanges { get; } = new List<Challange>();

    public virtual ICollection<Chat> ChatPersonReceivers { get; } = new List<Chat>();

    public virtual ICollection<Chat> ChatPersonSenders { get; } = new List<Chat>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<Feedback> FeedbackInterns { get; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackTrainers { get; } = new List<Feedback>();

    public virtual ICollection<GroupChatMessage> GroupChatMessages { get; } = new List<GroupChatMessage>();

    public virtual ICollection<GroupChatPerson> GroupChatPeople { get; } = new List<GroupChatPerson>();

    public virtual ICollection<GroupChat> GroupChats { get; } = new List<GroupChat>();

    public virtual ICollection<Group> Groups { get; } = new List<Group>();

    public virtual ICollection<InternGroup> InternGroups { get; } = new List<InternGroup>();

    public virtual ICollection<MeetingInternGroup> MeetingInternGroups { get; } = new List<MeetingInternGroup>();

    public virtual ICollection<Meeting> Meetings { get; } = new List<Meeting>();

    public virtual ICollection<Note> Notes { get; } = new List<Note>();

    public virtual Picture Picture { get; set; }

    public virtual ICollection<PostCommentReaction> PostCommentReactions { get; } = new List<PostCommentReaction>();

    public virtual ICollection<PostView> PostViews { get; } = new List<PostView>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();

    public virtual ICollection<QuestionSolution> QuestionSolutions { get; } = new List<QuestionSolution>();

    public virtual ICollection<Question> Questions { get; } = new List<Question>();

    public virtual ICollection<SubTaskChecked> SubTaskCheckeds { get; } = new List<SubTaskChecked>();

    public virtual ICollection<TaskInternGroup> TaskInternGroups { get; } = new List<TaskInternGroup>();

    public virtual ICollection<TaskSolution> TaskSolutions { get; } = new List<TaskSolution>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual ICollection<TestGroupIntern> TestGroupInterns { get; } = new List<TestGroupIntern>();

    public virtual ICollection<Test> Tests { get; } = new List<Test>();
}
