using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101747)]
    public class GroupTable : Migration
    {
        public const string tableName = "Group";
        public const string fkTrainerName = "GroupTrainerFK";
        public const string fkGroupPicture = "GroupPictureFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkTrainerName);
            Delete.ForeignKey(fkGroupPicture);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("GroupId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("GroupName").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().Nullable()
                .WithColumn("Description").AsString(int.MaxValue).NotNullable()
                .WithColumn("PictureId").AsInt32().Nullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkTrainerName)
               .FromTable(tableName).ForeignColumn("TrainerId")
               .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

            Create.ForeignKey(fkGroupPicture)
                .FromTable(tableName).ForeignColumn("PictureId")
                .ToTable(PictureTable.tableName).PrimaryColumn("PictureId");
        }
    }
}
