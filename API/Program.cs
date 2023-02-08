using System.Text.Json.Serialization;
using API.FluentMigration;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<SocialAppContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("dbconn")));
        builder.Services.AddCors();
        builder.Services.AddScoped<ITokenService,TokenService>();

        var app = builder.Build();

        Database.RunMigrations();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        app.UseAuthorization();
        //to change to work with http://localhost/4200
        app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

        app.UseHttpsRedirection();
        
        app.MapControllers();

        app.Run();
    }
}