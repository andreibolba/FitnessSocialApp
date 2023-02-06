using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060144)]
    public class AddPostCommentLike:Migration
    {
        public const string tableName = "PostCommentLike";
        public const string fkNamePerson = "PostCommentLikePersonFK";
        public const string fkNamePost = "PostCommentLikePostFK";
        public const string fkNameComment = "PostCommentLikeCommentFK";
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
                .WithColumn("PostId").AsInt32().NotNullable()
                .WithColumn("CommentId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePost)
                .FromTable(tableName).ForeignColumn("PostId")
                .ToTable(AddPost.tableName).PrimaryColumn("PostId");

            Create.ForeignKey(fkNameComment)
                .FromTable(tableName).ForeignColumn("CommentId")
                .ToTable(AddComment.tableName).PrimaryColumn("CommentId");
        }
    }
}