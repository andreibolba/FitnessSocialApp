using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101811)]
    public class TaskTable:Migration
    {
        public const string tableName = "Task";
        public const string fkNameGroup = "TaskGroupFK";
        public const string fkNameTrainer = "TaskrainerFK";
        public const string fkNameIntern = "TaskInternFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameGroup);
            Delete.ForeignKey(fkNameTrainer);
            Delete.ForeignKey(fkNameIntern);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TaskId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TaskName").AsString().NotNullable()
                .WithColumn("TaskDescription").AsString(int.MaxValue).NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deadline").AsDateTime().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable()
                .WithColumn("InternId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameGroup)
                .FromTable(tableName).ForeignColumn("GroupId")
                .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameTrainer)
                .FromTable(tableName).ForeignColumn("TrainerId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
