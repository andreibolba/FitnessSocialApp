
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly SocialAppContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SocialAppContext context)
        {
            _logger = logger;
            _context = context;
        }



        // private readonly SocialAppContext _context;

        // public WeatherForecastController(ILogger<WeatherForecastController> logger,SocialAppContext context)
        // {
        //     _logger = logger;
        //     _context = context;
        // }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Follow> Get()
        {
            var test = _context.Follows.Include(x=>x.PersonFollow).Include(x=>x.PersonFollowed).ToList();
            return test;
        }
    }
}