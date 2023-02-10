using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101758)]
    public class GroupChatPersonTable:Migration
    {
        public const string tableName = "GroupChatPerson";
        public const string fkNamePerson = "GroupChatPersonPersonFK";
        public const string fkNameGroupChat = "GroupChatPersonGrouChatFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
            Delete.ForeignKey(fkNameGroupChat);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupChatUserId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("GroupChatId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameGroupChat)
                .FromTable(tableName).ForeignColumn("GroupChatId")
                .ToTable(GroupChatTable.tableName).PrimaryColumn("GroupChatId");
        }
    }
}
