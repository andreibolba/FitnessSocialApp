using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101830)]
    public class QuestionTable : Migration
    {
        public const string tableName = "Question";
        public const string fkQuestionTrainer = "QuestionTrainerFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkQuestionTrainer);

        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("QuestionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("QuestionName").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().Nullable()
                .WithColumn("A").AsString().Nullable()
                .WithColumn("B").AsString().Nullable()
                .WithColumn("C").AsString().Nullable()
                .WithColumn("D").AsString().Nullable()
                .WithColumn("E").AsString().Nullable()
                .WithColumn("F").AsString().Nullable()
                .WithColumn("CorrectOption").AsString().NotNullable()
                .WithColumn("Points").AsInt32().NotNullable()
                .WithColumn("CanBeEdited").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkQuestionTrainer)
    .FromTable(tableName).ForeignColumn("TrainerId")
    .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
