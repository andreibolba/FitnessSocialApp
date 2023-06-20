using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101845)]
    public class FeedbackTable:Migration
    {
        public const string tableName = "Feedback";
        public const string fkNameSender= "FeedbackSenderFK";
        public const string fkNameReceiver = "FeedbackReceiverFK";
        public const string fkNameTask = "FeedbackTaskFK";
        public const string fkNameTest = "FeedbackTestFK";
        public const string fkNameChallange = "FeedbackChallangeFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameSender);
            Delete.ForeignKey(fkNameReceiver);
            Delete.ForeignKey(fkNameTask);
            Delete.ForeignKey(fkNameTest);
            Delete.ForeignKey(fkNameChallange);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("FeedbackId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonSenderId").AsInt32().NotNullable()
                .WithColumn("PersonReceiverId").AsInt32().NotNullable()
                .WithColumn("TaskId").AsInt32().Nullable()
                .WithColumn("ChallangeId").AsInt32().Nullable()
                .WithColumn("TestId").AsInt32().Nullable()
                .WithColumn("Content").AsString(int.MaxValue).NotNullable()
                .WithColumn("Grade").AsDouble().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameSender)
                .FromTable(tableName).ForeignColumn("PersonSenderId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameReceiver)
                .FromTable(tableName).ForeignColumn("PersonReceiverId")
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
