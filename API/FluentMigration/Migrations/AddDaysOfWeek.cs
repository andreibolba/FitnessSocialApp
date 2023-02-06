using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060100)]
    public class AddDaysOfWeek:Migration
    {
        public const string tableName = "DaysOfWeek";
        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("DaysOfWeekId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("DayName").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();   
        }
    }
}