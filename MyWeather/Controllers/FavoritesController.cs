using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWeather.Dtos;
using MyWeather.Models;

namespace MyWeather.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FavoritesController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IEnumerable<CityDto> GetFavoriteCities()
        {
            var favCities = _db.Favorites
                .Join(
                    _db.Cities,
                    fav => fav.CityId,
                    city => city.CityId,
                    (fav, city) => city
                )
                .OrderBy(c => c.CityName);

            List<CityDto> list = new List<CityDto>();
            foreach(var city in favCities)
            {
                list.Add(new CityDto()
                {
                    Key = city.Key,
                    Name = city.CityName,
                    Country = city.Country
                });
            }

            return list;
        }

        [HttpGet]
        public IEnumerable<Favorite> GetFavorites()
        {
            return _db.Favorites;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite(string key)
        {
            if(key == null)
            {
                return BadRequest();
            }

            City findCity = await _db.Cities.Where(c => c.Key == key).FirstOrDefaultAsync();
            if (findCity == null)
            {
                ModelState.AddModelError("", "City not exist!");
                return BadRequest(ModelState);
            }

            Favorite findFav = await _db.Favorites.Where(f => f.CityId == findCity.CityId).FirstOrDefaultAsync();
            if (findFav == null)
            {
                findFav = new Favorite()
                {
                    CityId = findCity.CityId
                };
                _db.Favorites.Add(findFav);
                await _db.SaveChangesAsync();
            }
            return Ok(findFav);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFavorite(string key)
        {
            if (key == null)
            {
                return BadRequest();
            }

            City findCity = await _db.Cities.Where(c => c.Key == key).FirstOrDefaultAsync();
            if (findCity == null)
            {
                ModelState.AddModelError("", "City not exist!");
                return BadRequest(ModelState);
            }

            Favorite findFav = await _db.Favorites.Where(f => f.CityId == findCity.CityId).FirstOrDefaultAsync();
            if (findFav == null)
            {
                return NotFound();
            }

            _db.Favorites.Remove(findFav);
            await _db.SaveChangesAsync();
            return Ok(findFav);
        }


    }
}