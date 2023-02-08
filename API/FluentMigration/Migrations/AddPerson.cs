using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060037)]
    public class AddPerson : Migration
    {
        public const string tableName = "Person";
        public override void Down()
        {
            Delete.Table(tableName);
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
                .WithColumn("BirthDate").AsDateTime().NotNullable()
                .WithColumn("IsAdmin").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}