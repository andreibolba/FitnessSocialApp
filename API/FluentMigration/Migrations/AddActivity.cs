using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060041)]
    public class AddActivity : Migration
    {
        public const string tableName = "Activity";
        public const string fkName = "ActivityPersonFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
               .WithColumn("ActivityId").AsInt32().PrimaryKey().NotNullable().Identity()
               .WithColumn("ActivityName").AsString().NotNullable()
               .WithColumn("AddedBy").AsInt32().NotNullable()
               .WithColumn("Deleted").AsBoolean().NotNullable();


            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("AddedBy")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}