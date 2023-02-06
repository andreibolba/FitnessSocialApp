using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060153)]
    public class AddProduct:Migration
    {
        public const string tableName = "Product";
        public const string fkName = "ProductPersonFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ProductId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("ProductName").AsString().NotNullable()
                .WithColumn("ProductPrice").AsDecimal().NotNullable()
                .WithColumn("ProductKalories").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}