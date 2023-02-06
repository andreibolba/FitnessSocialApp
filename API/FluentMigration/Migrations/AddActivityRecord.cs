using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060043)]
    public class AddActivityRecord : Migration
    {
        public const string tableName = "ActivityRecord";
        public const string fkPersonName = "ActivityRecordPersonFK";
        public const string fkActivityName = "ActivityRecordActivityFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkPersonName);
            Delete.ForeignKey(fkActivityName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ActivityRecordId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("ActivityId").AsInt32().NotNullable()
                .WithColumn("PersonId").AsInt32().NotNullable()              
                .WithColumn("Distance").AsDecimal().Nullable()              
                .WithColumn("BurnedKalories").AsInt32().NotNullable()              
                .WithColumn("Time").AsTime().Nullable()              
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkActivityName)
                .FromTable(tableName).ForeignColumn("ActivityId")
                .ToTable(AddActivity.tableName).PrimaryColumn("ActivityId");

            Create.ForeignKey(fkPersonName)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}