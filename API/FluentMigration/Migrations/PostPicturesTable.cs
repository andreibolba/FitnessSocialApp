using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    public class PostPicturesTable : Migration
    {
        public const string tableName = "PostPictures";
        public const string fkName = "PostPicturesPostFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostPicturesId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PostId").AsInt32().NotNullable()
                .WithColumn("Picture").AsBinary().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
               .FromTable(tableName).ForeignColumn("PostId")
               .ToTable(PostTable.tableName).PrimaryColumn("PostId");
        }
    }
}
