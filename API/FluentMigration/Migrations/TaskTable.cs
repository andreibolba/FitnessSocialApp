using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101811)]
    public class TaskTable:Migration
    {
        public const string tableName = "Task";
        public const string fkNameTrainer = "TaskrainerFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTrainer);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TaskId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TaskName").AsString().NotNullable()
                .WithColumn("TaskDescription").AsString(int.MaxValue).NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();


            Create.ForeignKey(fkNameTrainer)
                .FromTable(tableName).ForeignColumn("TrainerId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
