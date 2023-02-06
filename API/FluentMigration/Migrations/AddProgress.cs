using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060040)]
    public class AddProgress : Migration
    {
        public const string tableName = "Progress";
        public const string FKName = "PersonProgressFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(FKName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ProgressId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("BodyScore").AsInt32().NotNullable()
                .WithColumn("Weight").AsDecimal().NotNullable()
                .WithColumn("Water").AsDecimal().NotNullable()
                .WithColumn("BodyFat").AsDecimal().NotNullable()
                .WithColumn("VisceralFat").AsDecimal().NotNullable()
                .WithColumn("BMI").AsDecimal().NotNullable()
                .WithColumn("Muscle").AsDecimal().NotNullable()
                .WithColumn("Protein").AsDecimal().NotNullable()
                .WithColumn("BasalMetabolism").AsInt32().NotNullable()
                .WithColumn("BoneMass").AsDecimal().NotNullable()
                .WithColumn("BodyAge").AsInt32().NotNullable()
                .WithColumn("IdealWeight").AsDecimal().NotNullable()
                .WithColumn("Verdict").AsString().NotNullable()
                .WithColumn("DateRegister").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(FKName)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}