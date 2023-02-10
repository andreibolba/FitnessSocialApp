using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101822)]
    public class SubTaskCheckedTable:Migration
    {
        public const string tableName = "SubTaskChecked";
        public const string fkNameSubTask = "SubTaskCheckedSubTaskFK";
        public const string fkNameIntern = "SubTaskCheckedInternFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameSubTask);
            Delete.ForeignKey(fkNameIntern);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("SubTaskCheckedId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("SubTaskId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameSubTask)
                .FromTable(tableName).ForeignColumn("SubTaskId")
                .ToTable(SubTaskTable.tableName).PrimaryColumn("SubTaskId");

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
