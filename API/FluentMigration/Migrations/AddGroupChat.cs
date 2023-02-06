using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060204)]
    public class AddGroupChat:Migration
    {
        public const string tableName = "GroupChat";
        public const string fkName = "GroupChatPersonFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupChatId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupChatName").AsString().NotNullable()
                .WithColumn("Admin").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("Admin")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}