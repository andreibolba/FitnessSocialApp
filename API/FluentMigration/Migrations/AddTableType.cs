using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060217)]
    public class AddTableType:Migration
    {
        public const string tableName = "TableType";
        public override void Down()
        {
            Delete.Table(tableName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TableTypeId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TableTypeName").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();
        }
    }
}