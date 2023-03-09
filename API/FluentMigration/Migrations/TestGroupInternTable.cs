using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202303081416)]
    public class TestGroupInternTable : Migration
    {
        private const string tableName = "TestGroupIntern";
        private const string testGroupInternTestFK = "TestGroupInternTestFK";
        private const string testGroupInternGroupFK = "TestGroupInternGroupFK";
        private const string testGroupInternInternFK = "TestGroupInternInternFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(testGroupInternTestFK);
            Delete.ForeignKey(testGroupInternGroupFK);
            Delete.ForeignKey(testGroupInternTestFK);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TestGroupId").AsInt32().PrimaryKey().Unique().NotNullable().Identity()
                .WithColumn("TestId").AsInt32().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable()
                .WithColumn("InternId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(testGroupInternTestFK)
                .FromTable(tableName).ForeignColumn("TestId")
                .ToTable(TestTable.tableName).PrimaryColumn("TestId");

            Create.ForeignKey(testGroupInternGroupFK)
                .FromTable(tableName).ForeignColumn("GroupId")
                .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");

            Create.ForeignKey(testGroupInternInternFK)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");
        }
    }
}
