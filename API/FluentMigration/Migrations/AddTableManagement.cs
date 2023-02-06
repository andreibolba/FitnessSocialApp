using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060219)]
    public class AddTableManagement:Migration
    {
        public const string tableName = "TableManagement";
        public const string fkNamePerson = "TableManagementPersonFK";
        public const string fkNamePartOfDay = "TableManagementPartOfDayFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNamePartOfDay);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TableManagementId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TableTypeId").AsInt32().NotNullable()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("Quantity").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePartOfDay)
                .FromTable(tableName).ForeignColumn("TableTypeId")
                .ToTable(AddTableType.tableName).PrimaryColumn("TableTypeId");
        }
    }
}