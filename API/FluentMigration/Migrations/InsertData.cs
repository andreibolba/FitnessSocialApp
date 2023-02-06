using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.FluentMigration.Migrations
{
    [Migration(202302060228)]
    public class InsertData : Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Monday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Tuesday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Wednesday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Thursday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Friday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Saturday", Deleted = false });
            Insert.IntoTable(AddDaysOfWeek.tableName).Row(new { DayName = "Sunday", Deleted = false });

            Insert.IntoTable(AddTableType.tableName).Row(new { TableTypeName = "Breakfast", Deleted = false });
            Insert.IntoTable(AddTableType.tableName).Row(new { TableTypeName = "Lunch", Deleted = false });
            Insert.IntoTable(AddTableType.tableName).Row(new { TableTypeName = "Dinner", Deleted = false });
            Insert.IntoTable(AddTableType.tableName).Row(new { TableTypeName = "Snack", Deleted = false });

            Insert.IntoTable(AddPerson.tableName).Row(new { FirstName = "Bolba-Mateescu", LastName = "Andrei", Email = "andrei_bolba@yahoo.com", Username = "anreibolba", Password = "MTIzcXdlYXNk", BirthDate= new DateTime(2001, 7, 9), IsAdmin=false,Deleted = false });
            Insert.IntoTable(AddPerson.tableName).Row(new { FirstName = "Barna", LastName = "Lusia-Elena", Email = "barnalusia@yahoo.com", Username = "lusiabarna", Password = "MTIzcXdlYXNk", BirthDate= new DateTime(2001, 10, 30), IsAdmin=false,Deleted = false });
        }
    }
}