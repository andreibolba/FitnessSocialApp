using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060210)]
    public class AddGroupChatMessage:Migration
    {
        public const string tableName = "GroupChatMessage";
        public const string fkNamePerson = "GroupChatMessagePersonFK";
        public const string fkNameGroupChat = "GroupChatMessageGroupChatFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameGroupChat);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupChatMessageId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("GroupChatId").AsInt32().NotNullable()
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("SendDate").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameGroupChat)
                .FromTable(tableName).ForeignColumn("GroupChatId")
                .ToTable(AddGroupChat.tableName).PrimaryColumn("GroupChatId");
        }
    }
}