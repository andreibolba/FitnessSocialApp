using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101831)]
    public class QuestionSolutionTable:Migration
    {
        public const string tableName = "QuestionSolution";
        public const string fkNameQuestion = "QuestionSolutionQuestionFK";
        public const string fkNameIntern = "QuestionSolutionInternFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameQuestion);
            Delete.ForeignKey(fkNameIntern);

        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("QuestionSolutionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("QuestionId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("InternOption").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameQuestion)
                .FromTable(tableName).ForeignColumn("QuestionId")
                .ToTable(QuestionTable.tableName).PrimaryColumn("QuestionId");

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
