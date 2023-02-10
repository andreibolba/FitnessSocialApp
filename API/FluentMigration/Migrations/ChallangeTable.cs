using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101802)]
    public class ChallangeTable:Migration
    {
        public const string tableName = "Challange";
        public const string fkNameTrainer = "ChallangeTrainerFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTrainer);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ChallangeId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("ChallangeName").AsString().NotNullable()
                .WithColumn("ChallangeDescription").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deadline").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTrainer)
                .FromTable(tableName).ForeignColumn("TrainerId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
