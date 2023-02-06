using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060142)]
    public class AddPostPic:Migration
    {
        public const string tableName = "PostPic";
        public const string fkNamePost = "PostPicPostFK";
        public const string fkNameComment = "PostPicCommentFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePost);
            Delete.ForeignKey(fkNameComment);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostPicId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PostId").AsInt32().Nullable()
                .WithColumn("CommentId").AsInt32().Nullable()
                .WithColumn("Picture").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePost)
                .FromTable(tableName).ForeignColumn("PostId")
                .ToTable(AddPost.tableName).PrimaryColumn("PostId");

            Create.ForeignKey(fkNameComment)
                .FromTable(tableName).ForeignColumn("CommentId")
                .ToTable(AddComment.tableName).PrimaryColumn("CommentId");
        }
    }
}