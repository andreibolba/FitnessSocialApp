using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202303172155)]
    public class MeetingInternGroupTable : Migration
    {
        public const string tableName = "MeetingInternGroup";
        public const string fkMeetingMeeting = "MeetingInternGroupMeeting";
        public const string fkMeetingIntern = "MeetingInternGroupIntern";
        public const string fkMeetingGroup = "MeetingInternGroupGroup";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkMeetingMeeting);
            Delete.ForeignKey(fkMeetingIntern);
            Delete.ForeignKey(fkMeetingGroup);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("MeetingInternGroupId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("MeetingId").AsInt32().NotNullable()
                .WithColumn("InternId").AsInt32().Nullable()
                .WithColumn("GroupId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkMeetingMeeting)
               .FromTable(tableName).ForeignColumn("MeetingId")
               .ToTable(MeetingTable.tableName).PrimaryColumn("MeetingId");

            Create.ForeignKey(fkMeetingIntern)
               .FromTable(tableName).ForeignColumn("InternId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkMeetingGroup)
               .FromTable(tableName).ForeignColumn("GroupId")
               .ToTable(GroupTable.tableName).PrimaryColumn("GroupId");
        }
    }
}
