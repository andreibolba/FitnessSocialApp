using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060141)]
    public class AddComment:Migration
    {
        public const string tableName = "Comment";
        public const string fkNamePerson = "CommentePersonFK";
        public const string fkNamePost = "CommentePostFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNamePost);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("CommentId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("PostId").AsInt32().NotNullable()
                .WithColumn("CommentContent").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePost)
                .FromTable(tableName).ForeignColumn("PostId")
                .ToTable(AddPost.tableName).PrimaryColumn("PostId");
        }
    }
}