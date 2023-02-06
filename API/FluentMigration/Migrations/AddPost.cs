using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060120)]
    public class AddPost:Migration
    {
        public const string tableName = "Post";
        public const string fkNamePerson = "PostPersonFK";
        public const string fkNameProgress= "PostProgressFK";
        public const string fkNameExercise = "PostExerciseFK";
        public const string fkNameExercisePlan = "PostExercisePlanFK";
        public const string fkNameGroup = "PostGroupFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameProgress);
            Delete.ForeignKey(fkNameExercise);
            Delete.ForeignKey(fkNameExercisePlan);
            Delete.ForeignKey(fkNameGroup);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("ProgressId").AsInt32().Nullable()
                .WithColumn("ExerciseId").AsInt32().Nullable()
                .WithColumn("ExercisePlanningId").AsInt32().Nullable()
                .WithColumn("Content").AsString().Nullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameProgress)
                .FromTable(tableName).ForeignColumn("ProgressId")
                .ToTable(AddProgress.tableName).PrimaryColumn("ProgressId");

            Create.ForeignKey(fkNameExercise)
                .FromTable(tableName).ForeignColumn("ExerciseId")
                .ToTable(AddExercise.tableName).PrimaryColumn("ExerciseId");

            Create.ForeignKey(fkNameExercisePlan)
                .FromTable(tableName).ForeignColumn("ExercisePlanningId")
                .ToTable(AddExercisePlanning.tableName).PrimaryColumn("ExercisePlanningId");

            Create.ForeignKey(fkNameGroup)
                .FromTable(tableName).ForeignColumn("GroupId")
                .ToTable(AddGroup.tableName).PrimaryColumn("GroupId");
        }
    }
}