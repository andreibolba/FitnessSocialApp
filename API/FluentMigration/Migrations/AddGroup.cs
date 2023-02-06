using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060116)]
    public class AddGroup:Migration
    {
        public const string tableName = "Group";
        public const string fkName = "GroupAdminFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupName").AsString().NotNullable()
                .WithColumn("GroupDescription").AsString().NotNullable()
                .WithColumn("Admin").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("Admin")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}