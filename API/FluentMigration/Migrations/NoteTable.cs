using FluentMigrator;

namespace API.FluentMigration.Migrations
{
    [Migration(202302101800)]
    public class NoteTable:Migration
    {
        public const string tableName = "Note";
        public const string fkNamePerson = "NotePersonFK";

        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNamePerson);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("NoteId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("NoteTitle").AsString().NotNullable()
                .WithColumn("NoteBody").AsString().NotNullable()
                .WithColumn("PersonId").AsInt32().NotNullable()
                .WithColumn("PostingDate").AsDateTime().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNamePerson)
                .FromTable(tableName).ForeignColumn("PersonId")
                .ToTable(PersonTable.tableName).PrimaryColumn("PersonId");

        }
    }
}
