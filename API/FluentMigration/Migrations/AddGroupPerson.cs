using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060118)]
    public class AddGroupPerson:Migration
    {
        public const string tableName = "GroupPerson";
        public const string fkNamePerson = "GroupPersonPersonFK";
        public const string fkNameGroup = "GroupPersonGroupFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameGroup);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupPersonId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("GroupId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameGroup)
                .FromTable(tableName).ForeignColumn("GroupId")
                .ToTable(AddGroup.tableName).PrimaryColumn("GroupId");
        }
    }
}