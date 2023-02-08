using System.Text;
using System.Text.Json.Serialization;
using API.Extensions;
using API.FluentMigration;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);

        var app = builder.Build();

        //Database.RunMigrations();

        app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}