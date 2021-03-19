using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Models
{
    public class Weather
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public float Temperature { get; set; }

        [Required]
        public string WeatherText { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
