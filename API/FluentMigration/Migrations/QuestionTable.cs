using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101830)]
    public class QuestionTable:Migration
    {
        public const string tableName = "Question";
        public const string fkNameTest = "QuestionTestFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTest);

        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("QuestionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("QuestionName").AsString().NotNullable()
                .WithColumn("TestId").AsInt32().NotNullable()
                .WithColumn("A").AsString().Nullable()
                .WithColumn("B").AsString().Nullable()
                .WithColumn("C").AsString().Nullable()
                .WithColumn("D").AsString().Nullable()
                .WithColumn("E").AsString().Nullable()
                .WithColumn("F").AsString().Nullable()
                .WithColumn("CorrectOption").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTest)
                .FromTable(tableName).ForeignColumn("TestId")
                .ToTable(TestTable.tableName).PrimaryColumn("TestId");

        }
    }
}
