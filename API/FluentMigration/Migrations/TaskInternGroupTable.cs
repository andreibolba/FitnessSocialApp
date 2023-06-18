using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202306151923)]
    public class TaskInternGroupTable : Migration
    {
        public const string tableName = "TaskInternGroup";
        public const string fkNameTask = "TaskTaskInternGroupFK";
        public const string fkNameGroup = "TaskInternGroupGroupFK";
        public const string fkNameIntern = "TaskInternGroupInternFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameGroup);
            Delete.ForeignKey(fkNameTask);
            Delete.ForeignKey(fkNameIntern);
        }

        public override void Up()
        {
            Create.Table(tableName)
               .WithColumn("TaskInternGroupId").AsInt32().PrimaryKey().NotNullable().Identity()
               .WithColumn("TaskId").AsInt32().NotNullable()
               .WithColumn("InternId").AsInt32().Nullable()
               .WithColumn("GroupId").AsInt32().Nullable()
               .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTask)
               .FromTable(tableName).ForeignColumn("TaskId")
               .ToTable(TaskTable.tableName).PrimaryColumn("TaskId");

            Create.ForeignKey(fkNameIntern)
               .FromTable(tableName).ForeignColumn("InternId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameGroup)
              .FromTable(tableName).ForeignColumn("GroupId")
              .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");
        }
    }
}
