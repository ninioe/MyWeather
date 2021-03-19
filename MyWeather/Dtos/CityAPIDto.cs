using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Dtos
{
    public class CityAPIDto
    {

        public string Key { get; set; }

        public string Type { get; set; }

        public string LocalizedName { get; set; }

        public Country Country { get; set; }
    }

    public class Country
    {
        public string LocalizedName { get; set; }
    }
}
