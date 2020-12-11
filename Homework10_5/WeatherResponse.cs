using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework10_5
{
    class WeatherResponse
    {
        /// <summary>
        /// Класс, реализующий погоду
        /// </summary>
        public string Name { get; set; }
        public float Temp { get; set; }
        public float Feels_like { get; set; }
        public float Pressure { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
        public int Clouds { get; set; }
        public WeatherResponse(
            string name, 
            float temp, 
            float feels_like,
            float pressure,
            float humidity,
            float windSpeed,
            int clouds)
        {
            Name = name;
            Temp = temp;
            Feels_like = feels_like;
            Pressure = pressure;
            Humidity = humidity;
            WindSpeed = windSpeed;
            Clouds = clouds;
        }
    }
}
