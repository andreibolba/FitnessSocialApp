using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060109)]
    public class AddExercisePlanning:Migration
    {
        public const string tableName = "ExercisePlanning";
        public const string fkNamePerson = "ExercisePlanningPersonFK";
        public const string fkNameDays = "ExercisePlanningDaysFK";
        public const string fkNameExercise = "ExercisePlanningExerciseFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameDays);
            Delete.ForeignKey(fkNameExercise);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ExercisePlanningId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PlannerUserId").AsInt32().NotNullable()
                .WithColumn("DaysOfWeekId").AsInt32().NotNullable()
                .WithColumn("ExerciseId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PlannerUserId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameDays)
                .FromTable(tableName).ForeignColumn("DaysOfWeekId")
                .ToTable(AddDaysOfWeek.tableName).PrimaryColumn("DaysOfWeekId");

            Create.ForeignKey(fkNameExercise)
                .FromTable(tableName).ForeignColumn("ExerciseId")
                .ToTable(AddExercise.tableName).PrimaryColumn("ExerciseId");
        }
    }
}