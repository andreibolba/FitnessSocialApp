using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<SocialAppContext>(options =>
            options.UseSqlServer(config.GetConnectionString("dbconn")));
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}