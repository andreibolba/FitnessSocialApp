using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101744)]
    public class PersonTable:Migration
    {
        public const string tableName = "Person";
        public const string picturePersonFK = "PersonPictureFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.Table(picturePersonFK);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PersonId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("Email").AsString().Unique().NotNullable()
                .WithColumn("Username").AsString().Unique().NotNullable()
                .WithColumn("PasswordHash").AsBinary().NotNullable()
                .WithColumn("PasswordSalt").AsBinary().NotNullable()
                .WithColumn("Status").AsString().NotNullable()
                .WithColumn("Picture").AsString(int.MaxValue).Nullable()
                .WithColumn("BirthDate").AsDateTime().NotNullable()
                .WithColumn("PictureId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(picturePersonFK)
                .FromTable(tableName).ForeignColumn("PictureId")
                .ToTable(PictureTable.tableName).PrimaryColumn("PictureId");
        }
    }
}
