using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101750)]
    public class InternGroupTable:Migration
    {
        public const string tableName = "InternGroup";
        public const string fkGroupName = "InternGroupGroupFK";
        public const string fkInternName = "InternGroupInternFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkGroupName);
            Delete.ForeignKey(fkInternName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("InternGroupId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkGroupName)
               .FromTable(tableName).ForeignColumn("GroupId")
               .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");

            Create.ForeignKey(fkInternName)
               .FromTable(tableName).ForeignColumn("InternId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");
        }
    }
}
