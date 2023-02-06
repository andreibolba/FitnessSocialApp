using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060105)]
    public class AddExercise:Migration
    {
        public const string tableName = "Exercise";
        public const string fkNamePerson = "ExercisePersonFK";
        public const string fkNameMuscleGroup = "ExerciseMuscleGroupFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameMuscleGroup);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ExerciseId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("AddedBy").AsInt32().NotNullable()
                .WithColumn("MuscleGroupId").AsInt32().NotNullable()
                .WithColumn("ExerciseName").AsDateTime().NotNullable()
                .WithColumn("ExerciseDesription").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("AddedBy")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameMuscleGroup)
                .FromTable(tableName).ForeignColumn("MuscleGroupId")
                .ToTable(AddMuscleGroup.tableName).PrimaryColumn("MuscleGroupId");
        }
    }
}