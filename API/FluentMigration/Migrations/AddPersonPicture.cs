using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060054)]
    public class AddPersonPicture : Migration
    {
        public const string tableName = "PersonPicture";
        public const string fkName = "PersonPicturePersonFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("PersonPictureId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("Picture").AsString().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("ProfilePic").AsBoolean().NotNullable()
                .WithColumn("Actual").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}