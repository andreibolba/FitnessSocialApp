using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060158)]
    public class AddReview:Migration
    {
        public const string tableName = "Review";
        public const string fkNamePerson = "ReviewPersonFK";
        public const string fkNameExercise = "ReviewExerciseFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameExercise);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ReviewId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("ExerciseId").AsInt32().NotNullable()
                .WithColumn("Stars").AsInt32().NotNullable()
                .WithColumn("Comment").AsString().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameExercise)
                .FromTable(tableName).ForeignColumn("ExerciseId")
                .ToTable(AddExercise.tableName).PrimaryColumn("ExerciseId");
        }
    }
}