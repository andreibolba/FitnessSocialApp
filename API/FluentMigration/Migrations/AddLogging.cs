using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060222)]
    public class AddLogging:Migration
    {
        public const string tableName = "Log";
        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("LogId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("LogLevel").AsString().NotNullable()
                .WithColumn("Message").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}