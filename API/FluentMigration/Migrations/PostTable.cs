using FluentMigrator;
using System.Security.Cryptography;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101837)]
    public class PostTable:Migration
    {
        public const string tableName = "Post";
        public const string fkNamePerson = "PostPersonFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PostId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Content").AsString().Nullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
