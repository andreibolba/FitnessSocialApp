using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060155)]
    public class AddRecipeProduct:Migration
    {
        public const string tableName = "RecipeProduct";
        public const string fkNameRecipe = "RecipeProductRecipeFK";
        public const string fkNameProduct = "RecipeProductProductFK";
        public override void Down()
        {
            Delete.Table(tableName);
            Delete.ForeignKey(fkNameRecipe);
            Delete.ForeignKey(fkNameProduct);
        }

        public override void Up()
        {
            Create.Table(tableName)
                .WithColumn("RecipeProductId").AsInt32().PrimaryKey().NotNullable().Identity()
                .WithColumn("RecipeId").AsInt32().NotNullable()
                .WithColumn("ProductId").AsInt32().NotNullable()
                .WithColumn("Deleted").AsBoolean().NotNullable();

            Create.ForeignKey(fkNameRecipe)
                .FromTable(tableName).ForeignColumn("RecipeId")
                .ToTable(AddRecipe.tableName).PrimaryColumn("RecipeId");

            Create.ForeignKey(fkNameProduct)
                .FromTable(tableName).ForeignColumn("ProductId")
                .ToTable(AddProduct.tableName).PrimaryColumn("ProductId");
        }
    }
}