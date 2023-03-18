using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202303061311)]
    public class TestQuestionTable : Migration
    {
        public const string tableName = "TestQuestion";
        public const string fkTestQuestionTest = "TestQuestionTestK";
        public const string fkTestQuestionQuestion = "TestQuestionQuestionK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkTestQuestionTest);
            Delete.ForeignKey(fkTestQuestionQuestion);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TestQuestionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TestId").AsInt32().NotNullable()
                .WithColumn("QuestionId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkTestQuestionTest)
                .FromTable(tableName).ForeignColumn("TestId")
                .ToTable(TestTable.tableName).PrimaryColumn("TestId");

            Create.ForeignKey(fkTestQuestionQuestion)
                .FromTable(tableName).ForeignColumn("QuestionId")
                .ToTable(QuestionTable.tableName).PrimaryColumn("QuestionId");

        }
    }
}
