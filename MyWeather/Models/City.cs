using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
