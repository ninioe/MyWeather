using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Dtos
{
    public class WeatherDto
    {
        [Required]
        public int CityId { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public float Temperature { get; set; }

        [Required]
        public string WeatherText { get; set; }

        public bool IsFavorite { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
