using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101845)]
    public class FeedbackTable:Migration
    {
        public const string tableName = "Feedback";
        public const string fkNameIntern = "FeedbackInternFK";
        public const string fkNameTrainer = "FeedbackTrainerFK";
        public const string fkNameTask = "FeedbackTaskFK";
        public const string fkNameTest = "FeedbackTestFK";
        public const string fkNameChallange = "FeedbackChallangeFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameIntern);
            Delete.ForeignKey(fkNameTrainer);
            Delete.ForeignKey(fkNameTask);
            Delete.ForeignKey(fkNameTest);
            Delete.ForeignKey(fkNameChallange);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("FeedbackId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("TaskId").AsInt32().Nullable()
                .WithColumn("ChallangeId").AsInt32().Nullable()
                .WithColumn("TestId").AsInt32().Nullable()
                .WithColumn("Content").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameTrainer)
                .FromTable(tableName).ForeignColumn("TrainerId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameTask)
                .FromTable(tableName).ForeignColumn("TaskId")
                .ToTable(TaskTable.tableName).PrimaryColumn("TaskId");

            Create.ForeignKey(fkNameChallange)
                .FromTable(tableName).ForeignColumn("ChallangeId")
                .ToTable(ChallangeTable.tableName).PrimaryColumn("ChallangeId");

            Create.ForeignKey(fkNameTest)
                .FromTable(tableName).ForeignColumn("TestId")
                .ToTable(TestTable.tableName).PrimaryColumn("TestId");
        }
    }
}
