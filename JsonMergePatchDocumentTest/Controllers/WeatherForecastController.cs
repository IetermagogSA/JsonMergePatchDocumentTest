using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Newtonsoft.Json.Linq;
using JsonMergePatchDocumentTest.Models;
using Newtonsoft.Json;


namespace JsonMergePatchDocumentTest.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPatch]
        //[Route("[controller]/{id}")]
        public ProductOption Merge(int id,[FromBody] ProductOption updateOption)
        {
            ProductOption existingOption = new ProductOption
            {
                Id = Guid.Parse("be5ef465-e98f-4ff7-b51e-3fbdd2486116"),
                Name = "Existing Option",
                Description = "Existing Description",
                PriceIncl = 10.0M,
                OptionSizes = new List<OptionSize> 
                {
                    new OptionSize {Id = Guid.Parse("4fdbd44e-d3ce-4789-bb1a-cd862c6c791e"), Name = "Option 1", Width = 1, Height = 1},
                    new OptionSize {Id = Guid.Parse("a1561477-5dcc-4082-be5a-2f1ecedbbbe1"), Name = "Option 2", Height = 2, Width = 2},
                }
            };

            string existingOptionString = JsonConvert.SerializeObject(existingOption);
            JObject existingJObject = JObject.Parse(existingOptionString);

            string updateOptionString = JsonConvert.SerializeObject(updateOption);
            JObject updateJObject = JObject.Parse(updateOptionString);

            existingJObject.Merge(updateJObject, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Merge, MergeNullValueHandling = MergeNullValueHandling.Merge });

            return existingJObject.ToObject<ProductOption>();


            //// Original JSON object
            //var originalJson = "{\"name\": \"John\", \"surname\": \"Johnson\", \"age\": 25}";

            //var newJson = "{\"name\": \"Piet\", \"surname\":null}";

            //JObject j1 = JObject.Parse(originalJson);
            //JObject j2 = JObject.Parse(newJson);

            //j1.Merge(j2, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union, MergeNullValueHandling = MergeNullValueHandling.Merge });

            //Console.WriteLine("Patched JSON: " + j1);
        }
    }
}
