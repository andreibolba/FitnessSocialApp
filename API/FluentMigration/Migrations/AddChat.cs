using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060201)]
    public class AddChat:Migration
    {
        public const string tableName = "Chat";
        public const string fkNameSender = "ChatPersonSenderFK";
        public const string fkNameReceiver = "ChatPersonReceiverFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameSender);
            Delete.ForeignKey(fkNameReceiver);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ChatId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonSenderId").AsInt32().NotNullable()
                .WithColumn("PersonReceiverId").AsInt32().NotNullable()
                .WithColumn("Message").AsString().NotNullable()
                .WithColumn("SendDate").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameSender)
                .FromTable(tableName).ForeignColumn("PersonSenderId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameReceiver)
                .FromTable(tableName).ForeignColumn("PersonReceiverId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}