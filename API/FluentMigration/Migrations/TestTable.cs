using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101827)]
    public class TestTable:Migration
    {
        public const string tableName = "Test";
        public const string fkNameTrainer = "TestTrainerFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameTrainer);

        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("TestId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("TestName").AsString().NotNullable()
                .WithColumn("TrainerId").AsInt32().NotNullable()
                .WithColumn("DateOfPost").AsDateTime().NotNullable()
                .WithColumn("Deadline").AsDateTime().NotNullable()
                .WithColumn("CanBeDeleted").AsBoolean().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameTrainer)
                .FromTable(tableName).ForeignColumn("TrainerId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
