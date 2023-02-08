using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Models;

public partial class SocialAppContext : DbContext
{
    public SocialAppContext()
    {
    }

    public SocialAppContext(DbContextOptions<SocialAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityRecord> ActivityRecords { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DaysOfWeek> DaysOfWeeks { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<ExercisePlanning> ExercisePlannings { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupChat> GroupChats { get; set; }

    public virtual DbSet<GroupChatMessage> GroupChatMessages { get; set; }

    public virtual DbSet<GroupChatPerson> GroupChatPeople { get; set; }

    public virtual DbSet<GroupPerson> GroupPeople { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<MuscleGroup> MuscleGroups { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonPicture> PersonPictures { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostCommentLike> PostCommentLikes { get; set; }

    public virtual DbSet<PostPic> PostPics { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Progress> Progresses { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeProduct> RecipeProducts { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<TableManagement> TableManagements { get; set; }

    public virtual DbSet<TableType> TableTypes { get; set; }

    public virtual DbSet<VersionInfo> VersionInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=FitnessSocialApp;User Id=sa;Password=1234%asd; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.ToTable("Activity");

            entity.Property(e => e.ActivityName).HasMaxLength(255);

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.Activities)
                .HasForeignKey(d => d.AddedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActivityPersonFK");
        });

        modelBuilder.Entity<ActivityRecord>(entity =>
        {
            entity.ToTable("ActivityRecord");

            entity.Property(e => e.Distance).HasColumnType("decimal(19, 5)");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityRecords)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActivityRecordActivityFK");

            entity.HasOne(d => d.Person).WithMany(p => p.ActivityRecords)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ActivityRecordPersonFK");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Message).HasMaxLength(255);
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

            entity.Property(e => e.CommentContent).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CommentePersonFK");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CommentePostFK");
        });

        modelBuilder.Entity<DaysOfWeek>(entity =>
        {
            entity.ToTable("DaysOfWeek");

            entity.Property(e => e.DayName).HasMaxLength(255);
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.ToTable("Exercise");

            entity.Property(e => e.ExerciseName).HasColumnType("datetime");

            entity.HasOne(d => d.AddedByNavigation).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.AddedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExercisePersonFK");

            entity.HasOne(d => d.MuscleGroup).WithMany(p => p.Exercises)
                .HasForeignKey(d => d.MuscleGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExerciseMuscleGroupFK");
        });

        modelBuilder.Entity<ExercisePlanning>(entity =>
        {
            entity.ToTable("ExercisePlanning");

            entity.HasOne(d => d.DaysOfWeek).WithMany(p => p.ExercisePlannings)
                .HasForeignKey(d => d.DaysOfWeekId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExercisePlanningDaysFK");

            entity.HasOne(d => d.Exercise).WithMany(p => p.ExercisePlannings)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExercisePlanningExerciseFK");

            entity.HasOne(d => d.PlannerUser).WithMany(p => p.ExercisePlannings)
                .HasForeignKey(d => d.PlannerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ExercisePlanningPersonFK");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.ToTable("Follow");

            entity.HasOne(d => d.PersonFollow).WithMany(p => p.FollowPersonFollows)
                .HasForeignKey(d => d.PersonFollowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FollowPersonFK");

            entity.HasOne(d => d.PersonFollowed).WithMany(p => p.FollowPersonFolloweds)
                .HasForeignKey(d => d.PersonFollowedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FollowedPersonFK");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.ToTable("Group");

            entity.Property(e => e.GroupDescription).HasMaxLength(255);
            entity.Property(e => e.GroupName).HasMaxLength(255);

            entity.HasOne(d => d.AdminNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Admin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupAdminFK");
        });

        modelBuilder.Entity<GroupChat>(entity =>
        {
            entity.ToTable("GroupChat");

            entity.Property(e => e.GroupChatName).HasMaxLength(255);

            entity.HasOne(d => d.AdminNavigation).WithMany(p => p.GroupChats)
                .HasForeignKey(d => d.Admin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupChatPersonFK");
        });

        modelBuilder.Entity<GroupChatMessage>(entity =>
        {
            entity.ToTable("GroupChatMessage");

            entity.Property(e => e.Message).HasMaxLength(255);
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

        modelBuilder.Entity<GroupPerson>(entity =>
        {
            entity.ToTable("GroupPerson");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupPeople)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupPersonGroupFK");

            entity.HasOne(d => d.Person).WithMany(p => p.GroupPeople)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GroupPersonPersonFK");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.ToTable("Log");

            entity.Property(e => e.LogLevel).HasMaxLength(255);
            entity.Property(e => e.Message).HasColumnType("datetime");
        });

        modelBuilder.Entity<MuscleGroup>(entity =>
        {
            entity.ToTable("MuscleGroup");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("Person");

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(255);
            entity.Property(e => e.LastName).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<PersonPicture>(entity =>
        {
            entity.ToTable("PersonPicture");

            entity.Property(e => e.DateOfPost).HasColumnType("datetime");
            entity.Property(e => e.Picture).HasMaxLength(255);

            entity.HasOne(d => d.Person).WithMany(p => p.PersonPictures)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PersonPicturePersonFK");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.DateOfPost).HasColumnType("datetime");

            entity.HasOne(d => d.Exercise).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ExerciseId)
                .HasConstraintName("PostExerciseFK");

            entity.HasOne(d => d.ExercisePlanning).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ExercisePlanningId)
                .HasConstraintName("PostExercisePlanFK");

            entity.HasOne(d => d.Group).WithMany(p => p.Posts)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("PostGroupFK");

            entity.HasOne(d => d.Person).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostPersonFK");

            entity.HasOne(d => d.Progress).WithMany(p => p.Posts)
                .HasForeignKey(d => d.ProgressId)
                .HasConstraintName("PostProgressFK");
        });

        modelBuilder.Entity<PostCommentLike>(entity =>
        {
            entity.ToTable("PostCommentLike");

            entity.HasOne(d => d.Comment).WithMany(p => p.PostCommentLikes)
                .HasForeignKey(d => d.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentLikeCommentFK");

            entity.HasOne(d => d.Person).WithMany(p => p.PostCommentLikes)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentLikePersonFK");

            entity.HasOne(d => d.Post).WithMany(p => p.PostCommentLikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentLikePostFK");
        });

        modelBuilder.Entity<PostPic>(entity =>
        {
            entity.ToTable("PostPic");

            entity.Property(e => e.Picture).HasMaxLength(255);

            entity.HasOne(d => d.Comment).WithMany(p => p.PostPics)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("PostPicCommentFK");

            entity.HasOne(d => d.Post).WithMany(p => p.PostPics)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("PostPicPostFK");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(19, 5)");

            entity.HasOne(d => d.Person).WithMany(p => p.Products)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProductPersonFK");
        });

        modelBuilder.Entity<Progress>(entity =>
        {
            entity.ToTable("Progress");

            entity.Property(e => e.Bmi)
                .HasColumnType("decimal(19, 5)")
                .HasColumnName("BMI");
            entity.Property(e => e.BodyFat).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.BoneMass).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.DateRegister).HasColumnType("datetime");
            entity.Property(e => e.IdealWeight).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.Muscle).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.Protein).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.Verdict).HasMaxLength(255);
            entity.Property(e => e.VisceralFat).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.Water).HasColumnType("decimal(19, 5)");
            entity.Property(e => e.Weight).HasColumnType("decimal(19, 5)");

            entity.HasOne(d => d.Person).WithMany(p => p.Progresses)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PersonProgressFK");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("Recipe");

            entity.Property(e => e.RecipeDescription).HasMaxLength(255);
            entity.Property(e => e.RecipeName).HasMaxLength(255);

            entity.HasOne(d => d.Person).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RecipePersonFK");
        });

        modelBuilder.Entity<RecipeProduct>(entity =>
        {
            entity.ToTable("RecipeProduct");

            entity.HasOne(d => d.Product).WithMany(p => p.RecipeProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RecipeProductProductFK");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeProducts)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RecipeProductRecipeFK");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.ToTable("Review");

            entity.Property(e => e.Comment).HasMaxLength(255);

            entity.HasOne(d => d.Exercise).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReviewExerciseFK");

            entity.HasOne(d => d.Person).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ReviewPersonFK");
        });

        modelBuilder.Entity<TableManagement>(entity =>
        {
            entity.ToTable("TableManagement");

            entity.Property(e => e.Quantity).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.TableManagements)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TableManagementPersonFK");

            entity.HasOne(d => d.TableType).WithMany(p => p.TableManagements)
                .HasForeignKey(d => d.TableTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TableManagementPartOfDayFK");
        });

        modelBuilder.Entity<TableType>(entity =>
        {
            entity.ToTable("TableType");

            entity.Property(e => e.TableTypeName).HasMaxLength(255);
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
