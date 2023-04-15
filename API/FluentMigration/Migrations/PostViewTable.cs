using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202304151620)]
    public class PostViewTable : Migration
    {
        public const string tableName = "PostView";
        public const string fkPersonName = "PostViewPersonFK";
        public const string fkPostName = "PostViewPostFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkPersonName);
            Delete.ForeignKey(fkPostName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostViewId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("PostId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkPersonName)
               .FromTable(tableName).ForeignColumn("PersonId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkPostName)
                .FromTable(tableName).ForeignColumn("PostId")
                .ToTable(PostTable.tableName).PrimaryColumn("PostId");
        }
    }
}
