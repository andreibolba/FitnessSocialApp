using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060104)]
    public class AddMuscleGroup:Migration
    {
        public const string tableName = "MuscleGroup";

        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("MuscleGroupId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("MuscleGroupName").AsInt32().NotNullable()
                .WithColumn("MuscleGroupDescription").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}