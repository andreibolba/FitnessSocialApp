

using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101816)]
    public class TaskSolutionTable:Migration
    {
        public const string tableName = "TaskSolution";
        public const string fkNameTask = "TaskSolutionTaskFK";
        public const string fkNameIntern = "TaskSolutionInternFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTask);
            Delete.ForeignKey(fkNameIntern);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TaskSolutionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TaskId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("SolutionLink").AsString().NotNullable()
                .WithColumn("DateOfSolution").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTask)
                .FromTable(tableName).ForeignColumn("TaskId")
                .ToTable(TaskTable.tableName).PrimaryColumn("TaskId");

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");


        }
    }
}
