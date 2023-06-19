using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101044)]
    public class PictureTable:Migration
    {
        public const string tableName = "Picture";

        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PictureId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("Url").AsString().NotNullable()
                .WithColumn("PublicId").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

        }
    }
}
