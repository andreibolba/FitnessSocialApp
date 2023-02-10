using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101820)]
    public class SubTaskTable:Migration
    {
        public const string tableName = "SubTask";
        public const string fkNameTask = "SubTaskTaskFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTask);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("SubTaskId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("SubTaskName").AsString().NotNullable()
                .WithColumn("TaskId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTask)
                .FromTable(tableName).ForeignColumn("TaskId")
                .ToTable(TaskTable.tableName).PrimaryColumn("TaskId");

        }
    }
}
