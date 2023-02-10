using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101805)]
    public class ChallangeSolutionTable:Migration
    {
        public const string tableName = "ChallangeSolution";
        public const string fkNameChallange = "ChallangeSolutionChallangeFK";
        public const string fkNameIntern = "ChallangeSolutionInternFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameChallange);
            Delete.ForeignKey(fkNameIntern);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("ChallangeSolutionId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("ChallangeId").AsInt32().NotNullable()
                .WithColumn("DateOfSolution").AsDateTime().NotNullable()
                .WithColumn("InternId").AsInt32().NotNullable()
                .WithColumn("SolutionLink").AsString().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameChallange)
                .FromTable(tableName).ForeignColumn("ChallangeId")
                .ToTable(ChallangeTable.tableName).PrimaryColumn("ChallangeId");

            Create.ForeignKey(fkNameIntern)
                .FromTable(tableName).ForeignColumn("InternId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
