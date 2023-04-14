using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class InternShipAppSystemContext : DbContext
{
    public InternShipAppSystemContext()
    {
    }

    public InternShipAppSystemContext(DbContextOptions<InternShipAppSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Challange> Challanges { get; set; }

    public virtual DbSet<ChallangeSolution> ChallangeSolutions { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<GetPeopleInGroupMeeting> GetPeopleInGroupMeetings { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupChat> GroupChats { get; set; }

    public virtual DbSet<GroupChatMessage> GroupChatMessages { get; set; }

    public virtual DbSet<GroupChatPerson> GroupChatPeople { get; set; }

    public virtual DbSet<InternGroup> InternGroups { get; set; }

    public virtual DbSet<Logging> Loggings { get; set; }

    public virtual DbSet<Meeting> Meetings { get; set; }

    public virtual DbSet<MeetingInternGroup> MeetingInternGroups { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<PasswordkLink> PasswordkLinks { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCommentReaction> PostCommentReactions { get; set; }

    public virtual DbSet<PostPicture> PostPictures { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionSolution> QuestionSolutions { get; set; }

    public virtual DbSet<SubTask> SubTasks { get; set; }

    public virtual DbSet<SubTaskChecked> SubTaskCheckeds { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskSolution> TaskSolutions { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<TestGroupIntern> TestGroupInterns { get; set; }

    public virtual DbSet<TestQuestion> TestQuestions { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=InternShipAppSystem;User Id=sa;Password=1234%asd; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Challange>(entity =>
        {
            entity.ToTable("Challange");

            entity.Property(e => e.ChallangeDescription)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.ChallangeName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateOfPost).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Challanges)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ChallangeTrainerFK");
        });

        modelBuilder.Entity<ChallangeSolution>(entity =>
        {
            entity.ToTable("ChallangeSolution");

            entity.Property(e => e.DateOfSolution).HasColumnType("datetime");
            entity.Property(e => e.SolutionLink)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Challange).WithMany(p => p.ChallangeSolutions)
                .HasForeignKey(d => d.ChallangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ChallangeSolutionChallangeFK");

            entity.HasOne(d => d.Intern).WithMany(p => p.ChallangeSolutions)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ChallangeSolutionInternFK");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.PersonReceiver).WithMany(p => p.ChatPersonReceivers)
                .HasForeignKey(d => d.PersonReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ChatPersonReceiverFK");

            entity.HasOne(d => d.PersonSender).WithMany(p => p.ChatPersonSenders)
                .HasForeignKey(d => d.PersonSenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ChatPersonSenderFK");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comment");

            entity.Property(e => e.CommentContent)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.DateOfComment).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CommentePersonFK");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CommentePostFK");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");

            entity.HasOne(d => d.Challange).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ChallangeId)
                .HasConstraintName("FeedbackChallangeFK");

            entity.HasOne(d => d.Intern).WithMany(p => p.FeedbackInterns)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FeedbackInternFK");

            entity.HasOne(d => d.Task).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TaskId)
                .HasConstraintName("FeedbackTaskFK");

            entity.HasOne(d => d.Test).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FeedbackTestFK");

            entity.HasOne(d => d.Trainer).WithMany(p => p.FeedbackTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FeedbackTrainerFK");
        });

        modelBuilder.Entity<GetPeopleInGroupMeeting>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("GetPeopleInGroupMeeting");

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(8000);
            entity.Property(e => e.PasswordSalt).HasMaxLength(8000);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.Property(e => e.GroupName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Trainer).WithMany(p => p.Groups)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("GroupTrainerFK");
        });

        modelBuilder.Entity<GroupChat>(entity =>
        {
            entity.ToTable("GroupChat");

            entity.Property(e => e.GroupChatName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Admin).WithMany(p => p.GroupChats)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatPersonFK");
        });

        modelBuilder.Entity<GroupChatMessage>(entity =>
        {
            entity.ToTable("GroupChatMessage");

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.SendDate).HasColumnType("datetime");

            entity.HasOne(d => d.GroupChat).WithMany(p => p.GroupChatMessages)
                .HasForeignKey(d => d.GroupChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatMessageGroupChatFK");

            entity.HasOne(d => d.Person).WithMany(p => p.GroupChatMessages)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatMessagePersonFK");
        });

        modelBuilder.Entity<GroupChatPerson>(entity =>
        {
            entity.HasKey(e => e.GroupChatUserId);

            entity.ToTable("GroupChatPerson");

            entity.HasOne(d => d.GroupChat).WithMany(p => p.GroupChatPeople)
                .HasForeignKey(d => d.GroupChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatPersonGrouChatFK");

            entity.HasOne(d => d.Person).WithMany(p => p.GroupChatPeople)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatPersonPersonFK");
        });

        modelBuilder.Entity<InternGroup>(entity =>
        {
            entity.ToTable("InternGroup");

            entity.HasOne(d => d.Group).WithMany(p => p.InternGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InternGroupGroupFK");

            entity.HasOne(d => d.Intern).WithMany(p => p.InternGroups)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("InternGroupInternFK");
        });

        modelBuilder.Entity<Logging>(entity =>
        {
            entity.ToTable("Logging");

            entity.Property(e => e.DateOfLog).HasColumnType("datetime");
            entity.Property(e => e.LogMessage)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.LogType)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PersonUsername).HasMaxLength(255);
        });

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.ToTable("Meeting");

            entity.Property(e => e.MeetingFinishTime).HasColumnType("datetime");
            entity.Property(e => e.MeetingLink)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.MeetingName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.MeetingStartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Trainer).WithMany(p => p.Meetings)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MeetingTrainerFK");
        });

        modelBuilder.Entity<MeetingInternGroup>(entity =>
        {
            entity.ToTable("MeetingInternGroup");

            entity.HasOne(d => d.Group).WithMany(p => p.MeetingInternGroups)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("MeetingInternGroupGroup");

            entity.HasOne(d => d.Intern).WithMany(p => p.MeetingInternGroups)
                .HasForeignKey(d => d.InternId)
                .HasConstraintName("MeetingInternGroupIntern");

            entity.HasOne(d => d.Meeting).WithMany(p => p.MeetingInternGroups)
                .HasForeignKey(d => d.MeetingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MeetingInternGroupMeeting");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.Property(e => e.NoteBody)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.NoteTitle)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PostingDate).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Notes)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NotePersonFK");
        });

        modelBuilder.Entity<PasswordkLink>(entity =>
        {
            entity.HasKey(e => e.PasswordLinkId);

            entity.ToTable("PasswordkLink");

            entity.Property(e => e.PersonUsername)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Time).HasColumnType("datetime");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.HasIndex(e => e.Email, "IX_Person_Email").IsUnique();

            entity.HasIndex(e => e.Username, "IX_Person_Username").IsUnique();

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(8000);
            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(8000);
            entity.Property(e => e.Picture).HasMaxLength(8000);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.DateOfPost).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostPersonFK");
        });

        modelBuilder.Entity<PostCommentReaction>(entity =>
        {
            entity.HasKey(e => e.PostCommentLikeId);

            entity.HasOne(d => d.Comment).WithMany(p => p.PostCommentReactions)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("PostCommentReactionsCommentFK");

            entity.HasOne(d => d.Person).WithMany(p => p.PostCommentReactions)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentReactionsPersonFK");

            entity.HasOne(d => d.Post).WithMany(p => p.PostCommentReactions)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("PostCommentReactionsPostFK");
        });

        modelBuilder.Entity<PostPicture>(entity =>
        {
            entity.HasKey(e => e.PostPicturesId);

            entity.Property(e => e.Picture).HasMaxLength(8000);

            entity.HasOne(d => d.Post).WithMany(p => p.PostPictures)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostPicturesPostFK");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question");

            entity.Property(e => e.A).HasMaxLength(255);
            entity.Property(e => e.B).HasMaxLength(255);
            entity.Property(e => e.C).HasMaxLength(255);
            entity.Property(e => e.CorrectOption)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.D).HasMaxLength(255);
            entity.Property(e => e.E).HasMaxLength(255);
            entity.Property(e => e.F).HasMaxLength(255);
            entity.Property(e => e.QuestionName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Trainer).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TrainerId)
                .HasConstraintName("QuestionTrainerFK");
        });

        modelBuilder.Entity<QuestionSolution>(entity =>
        {
            entity.ToTable("QuestionSolution");

            entity.Property(e => e.InternOption)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Intern).WithMany(p => p.QuestionSolutions)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionSolutionInternFK");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionSolutions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionSolutionQuestionFK");

            entity.HasOne(d => d.Test).WithMany(p => p.QuestionSolutions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("QuestionSolutionTestFK");
        });

        modelBuilder.Entity<SubTask>(entity =>
        {
            entity.ToTable("SubTask");

            entity.Property(e => e.SubTaskName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Task).WithMany(p => p.SubTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubTaskTaskFK");
        });

        modelBuilder.Entity<SubTaskChecked>(entity =>
        {
            entity.ToTable("SubTaskChecked");

            entity.HasOne(d => d.Intern).WithMany(p => p.SubTaskCheckeds)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubTaskCheckedInternFK");

            entity.HasOne(d => d.SubTask).WithMany(p => p.SubTaskCheckeds)
                .HasForeignKey(d => d.SubTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubTaskCheckedSubTaskFK");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("Task");

            entity.Property(e => e.DateOfPost).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.TaskDescription)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.TaskName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Group).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("TaskGroupFK");

            entity.HasOne(d => d.Intern).WithMany(p => p.TaskInterns)
                .HasForeignKey(d => d.InternId)
                .HasConstraintName("TaskInternFK");

            entity.HasOne(d => d.Trainer).WithMany(p => p.TaskTrainers)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TaskrainerFK");
        });

        modelBuilder.Entity<TaskSolution>(entity =>
        {
            entity.ToTable("TaskSolution");

            entity.Property(e => e.DateOfSolution).HasColumnType("datetime");
            entity.Property(e => e.SolutionLink)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Intern).WithMany(p => p.TaskSolutions)
                .HasForeignKey(d => d.InternId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TaskSolutionInternFK");

            entity.HasOne(d => d.Task).WithMany(p => p.TaskSolutions)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TaskSolutionTaskFK");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.ToTable("Test");

            entity.Property(e => e.DateOfPost).HasColumnType("datetime");
            entity.Property(e => e.Deadline).HasColumnType("datetime");
            entity.Property(e => e.TestName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(d => d.Trainer).WithMany(p => p.Tests)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestTrainerFK");
        });

        modelBuilder.Entity<TestGroupIntern>(entity =>
        {
            entity.HasKey(e => e.TestGroupId);

            entity.ToTable("TestGroupIntern");

            entity.HasIndex(e => e.TestGroupId, "IX_TestGroupIntern_TestGroupId").IsUnique();

            entity.HasOne(d => d.Group).WithMany(p => p.TestGroupInterns)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("TestGroupInternGroupFK");

            entity.HasOne(d => d.Intern).WithMany(p => p.TestGroupInterns)
                .HasForeignKey(d => d.InternId)
                .HasConstraintName("TestGroupInternInternFK");

            entity.HasOne(d => d.Test).WithMany(p => p.TestGroupInterns)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestGroupInternTestFK");
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.ToTable("TestQuestion");

            entity.HasOne(d => d.Question).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestQuestionQuestionK");

            entity.HasOne(d => d.Test).WithMany(p => p.TestQuestions)
                .HasForeignKey(d => d.TestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TestQuestionTestK");
        });

        modelBuilder.Entity<VersionInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("VersionInfo");

            entity.HasIndex(e => e.Version, "UC_Version")
                .IsUnique()
                .IsClustered();

            entity.Property(e => e.AppliedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1024);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
