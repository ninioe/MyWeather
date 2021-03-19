using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWeather.Dtos
{
    public class WeatherAPIDto
	{
		public string WeatherText { get; set; }
		public int WeatherIcon { get; set; }
		public Temperature Temperature { get; set; }
    }

	public class Temperature
    {
        public Metric Metric { get; set; }
    }

	public class Metric
    {
        public float Value { get; set; }
    }
}
