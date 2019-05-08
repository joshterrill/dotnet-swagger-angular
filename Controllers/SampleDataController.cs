using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.CodeGeneration.TypeScript;

namespace dotnetangular.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("generate")]
        public ContentResult Generate() {
          
            var document = SwaggerDocument.FromUrlAsync("https://localhost:5001/swagger/v1/swagger.json");

            var settings = new SwaggerToTypeScriptClientGeneratorSettings
            {
              ClassName = "{controller}Client",
              Template = TypeScriptTemplate.Angular,
              InjectionTokenType = InjectionTokenType.InjectionToken
            };

            var generator = new SwaggerToTypeScriptClientGenerator(document.Result, settings);
            var code = generator.GenerateFile();

            System.IO.File.WriteAllText(@"./ClientApp/src/app/app.service.ts", code);

            return Content(code, "text/plain");
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
