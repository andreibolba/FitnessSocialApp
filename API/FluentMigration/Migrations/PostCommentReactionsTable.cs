using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101839)]
    public class PostCommentReactionsTable:Migration
    {
        public const string tableName = "PostCommentReactions";
        public const string fkNamePerson = "PostCommentReactionsPersonFK";
        public const string fkNamePost = "PostCommentReactionsPostFK";
        public const string fkNameComment = "PostCommentReactionsCommentFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNamePost);
            Delete.ForeignKey(fkNameComment);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostCommentLikeId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("PostId").AsInt32().Nullable()
                .WithColumn("CommentId").AsInt32().Nullable()
                .WithColumn("Upvote").AsBoolean().Nullable()
                .WithColumn("DownVote").AsBoolean().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePost)
                .FromTable(tableName).ForeignColumn("PostId")
                .ToTable(PostTable.tableName).PrimaryColumn("PostId");

            Create.ForeignKey(fkNameComment)
                .FromTable(tableName).ForeignColumn("CommentId")
                .ToTable(CommentTable.tableName).PrimaryColumn("CommentId");
        }
    }
}
