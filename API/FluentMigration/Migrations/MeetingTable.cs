using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101752)]
    public class MeetingTable:Migration
    {
        public const string tableName = "Meeting";
        public const string fkTrainerName = "MeetingTrainerFK";
        public const string fkGroupName = "MeetingGroupFK";
        public const string fkInternName = "MeetingInternFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkTrainerName);
            Delete.ForeignKey(fkGroupName);
            Delete.ForeignKey(fkInternName);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("MeetingId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("MeetingName").AsString().NotNullable()
                .WithColumn("MeetingLink").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable()
                .WithColumn("InternId").AsInt32().Nullable()
                .WithColumn("MeetingStartTime").AsDateTime().NotNullable()
                .WithColumn("MeetingFinishTime").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkTrainerName)
               .FromTable(tableName).ForeignColumn("TrainerId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkGroupName)
               .FromTable(tableName).ForeignColumn("GroupId")
               .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");

            Create.ForeignKey(fkInternName)
               .FromTable(tableName).ForeignColumn("InternId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");
        }
    }
}
