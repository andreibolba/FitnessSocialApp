using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101757)]
    public class GroupChatTable:Migration
    {
        public const string tableName = "GroupChat";
        public const string fkName = "GroupChatPersonFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupChatId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupChatName").AsString().NotNullable()
                .WithColumn("GroupChatDescription").AsString(int.MaxValue).Nullable()
                .WithColumn("AdminId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("AdminId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");
        }
    }
}
