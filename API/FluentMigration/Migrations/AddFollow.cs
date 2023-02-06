using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060057)]
    public class AddFollow:Migration
    {
        public const string tableName = "Follow";
        public const string fkNameFollow = "FollowPersonFK";
        public const string fkNameFollowed = "FollowedPersonFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameFollow);
            Delete.ForeignKey(fkNameFollowed);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("FollowId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonFollowId").AsInt32().NotNullable()
                .WithColumn("PersonFollowedId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameFollow)
                .FromTable(tableName).ForeignColumn("PersonFollowId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameFollowed)
                .FromTable(tableName).ForeignColumn("PersonFollowedId")
                .ToTable(AddPerson.tableName).PrimaryColumn("PersonId");
        }
    }
}