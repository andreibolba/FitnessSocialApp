using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101752)]
    public class MeetingTable:Migration
    {
        public const string tableName = "Meeting";
        public const string fkTrainerName = "MeetingTrainerFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkTrainerName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("MeetingId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("MeetingName").AsString().NotNullable()
                .WithColumn("MeetingLink").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("MeetingStartTime").AsDateTime().NotNullable()
                .WithColumn("MeetingFinishTime").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkTrainerName)
               .FromTable(tableName).ForeignColumn("TrainerId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");
        }
    }
}
