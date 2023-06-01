using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302112232)]
    public class LoggingTable:Migration
    {
        public const string tableName = "Logging";
        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("LoggingId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("LogType").AsString().NotNullable()
                .WithColumn("LogMessage").AsString(int.MaxValue).NotNullable()
                .WithColumn("PersonUsername").AsString().Nullable()
                .WithColumn("DateOfLog").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}