using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101757)]
    public class GroupChatTable:Migration
    {
        public const string tableName = "GroupChat";
        public const string fkName = "GroupChatPersonFK";
        public const string fkNamePicture = "GroupChatPictureFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkName);
            Delete.ForeignKey(fkNamePicture);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupChatId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupChatName").AsString().NotNullable()
                .WithColumn("GroupChatDescription").AsString(int.MaxValue).Nullable()
                .WithColumn("AdminId").AsInt32().NotNullable()
                .WithColumn("PictureId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkName)
                .FromTable(tableName).ForeignColumn("AdminId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePicture)
               .FromTable(tableName).ForeignColumn("PictureId")
               .ToTable(PictureTable.tableName).PrimaryColumn("PictureId");
        }
    }
}
