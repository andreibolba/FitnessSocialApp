using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101852)]
    public class PasswordkLinkTable:Migration
    {
        public const string tableName = "PasswordkLink";

        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PasswordLinkId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonUsername").AsInt32().NotNullable()
                .WithColumn("Time").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}
