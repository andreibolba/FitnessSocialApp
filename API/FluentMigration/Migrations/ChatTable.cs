using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101756)]
    public class ChatTable:Migration
    {
        public const string tableName = "Chat";
        public const string fkNameSender = "ChatPersonSenderFK";
        public const string fkNameReceiver = "ChatPersonReceiverFK";
        public const string fkNamePicture = "ChatPersonPictureFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameSender);
            Delete.ForeignKey(fkNameReceiver);
            Delete.ForeignKey(fkNamePicture);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ChatId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("PersonSenderId").AsInt32().NotNullable()
                .WithColumn("PersonReceiverId").AsInt32().NotNullable()
                .WithColumn("Message").AsString(int.MaxValue).NotNullable()
                .WithColumn("SendDate").AsDateTime().NotNullable()
                .WithColumn("PictureId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameSender)
                .FromTable(tableName).ForeignColumn("PersonSenderId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNameReceiver)
                .FromTable(tableName).ForeignColumn("PersonReceiverId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkNamePicture)
                .FromTable(tableName).ForeignColumn("PictureId")
                .ToTable(PictureTable.tableName).PrimaryColumn("PictureId");
        }
    }
}
