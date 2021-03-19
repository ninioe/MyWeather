using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyWeather.Dtos;
using MyWeather.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWeather.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private ApplicationDbContext _db;
        private string apikey;
        public CityController(IConfiguration configuration, ApplicationDbContext db)
        {
            Configuration = configuration;
            _db = db;
            apikey = Configuration["apikey"];
        }

        public IConfiguration Configuration { get; set; }

        private WeatherAPIDto getWeatherFromAPI(string key)
        {
            WeatherAPIDto weatherObj = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://dataservice.accuweather.com/currentconditions/v1/");
                //HTTP GET
                var responseTask = client.GetAsync($"{key}?apikey={apikey}");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<WeatherAPIDto[]>();
                    readTask.Wait();
                    weatherObj = readTask.Result.First();
                }
            }

            return weatherObj;
        }

        [HttpGet]
        [ActionName("Search")]
        public IActionResult Search(string term)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://dataservice.accuweather.com/locations/v1/");
                    //HTTP GET
                    var responseTask = client.GetAsync($"cities/autocomplete?apikey={apikey}&q={term}");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        var readTask = result.Content.ReadAsAsync<CityAPIDto[]>();
                        readTask.Wait();

                        var cities = readTask.Result;

                        List<CityDto> citiesObj = new List<CityDto>();
                        foreach (var city in cities)
                        {
                            citiesObj.Add(new CityDto
                            {
                                Key = city.Key,
                                Type = city.Type,
                                Name = city.LocalizedName,
                                Country = city.Country.LocalizedName
                            });
                        }

                        return Ok(citiesObj);
                    }
                }
            }
            catch { }


            return StatusCode(500, ModelState);
        }


        [HttpPost]
        public IActionResult GetCurrentWeather(CityDto city)
        {
            City findCity = _db.Cities.Where(c => c.Key == city.Key).FirstOrDefault();
            if (findCity == null)
            {
                findCity = new City()
                {
                    CityName = city.Name,
                    Key = city.Key,
                    Country = city.Country
                };
                _db.Cities.Add(findCity);
                _db.SaveChanges();
            }

            Weather findWeather = _db.Weathers.Where(w => w.Key == city.Key).FirstOrDefault();
            if (findWeather != null && findWeather.LastUpdate.AddDays(1) > DateTime.Now)
            {
                Favorite fav = _db.Favorites.Where(f => f.CityId == findCity.CityId).FirstOrDefault();
                WeatherDto weatherObj = new WeatherDto()
                {
                    CityName = city.Name,
                    Country = city.Country,
                    Key = city.Key,
                    Temperature = findWeather.Temperature,
                    WeatherText = findWeather.WeatherText,
                    IsFavorite = fav != null
                };
                return Ok(weatherObj);
            }

            try
            {
                WeatherAPIDto weatherAPIObj = getWeatherFromAPI(city.Key);
                if (findWeather != null)
                {
                    findWeather.Temperature = weatherAPIObj.Temperature.Metric.Value;
                    findWeather.WeatherText = weatherAPIObj.WeatherText;
                    _db.Update(findWeather);
                }
                else
                {
                    findWeather = new Weather()
                    {
                        CityId = findCity.CityId,
                        Key = findCity.Key,
                        Temperature = weatherAPIObj.Temperature.Metric.Value,
                        WeatherText = weatherAPIObj.WeatherText,
                        LastUpdate = DateTime.Now
                    };
                    _db.Add(findWeather);
                }
                _db.SaveChanges();

                Favorite fav = _db.Favorites.Where(f => f.CityId == findCity.CityId).FirstOrDefault();
                WeatherDto weatherObj = new WeatherDto()
                {
                    CityName = city.Name,
                    Country = city.Country,
                    Key = city.Key,
                    Temperature = findWeather.Temperature,
                    WeatherText = findWeather.WeatherText,
                    IsFavorite = fav != null
                };
                return Ok(weatherObj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return StatusCode(500, ModelState);
        }

    }
}
