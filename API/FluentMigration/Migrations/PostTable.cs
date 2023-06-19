using FluentMigrator;
using System.Security.Cryptography;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101837)]
    public class PostTable:Migration
    {
        public const string tableName = "Post";
        public const string fkNamePerson = "PostPersonFK";
        public const string fkPhotoPerson = "PostPhotoFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkPhotoPerson);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Content").AsString(int.MaxValue).Nullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("PictureId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkPhotoPerson)
                .FromTable(tableName).ForeignColumn("PictureId")
                .ToTable(PictureTable.tableName).PrimaryColumn("PictureId");

        }
    }
}
