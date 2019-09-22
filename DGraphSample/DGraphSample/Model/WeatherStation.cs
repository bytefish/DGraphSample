using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DGraphSample.Model
{

    public class WeatherStationList
    {
        [JsonProperty("stations")]
        public WeatherStation[] Stations { get; set; }
    }


    public class WeatherStation
    {
        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("weather_station.name")]
        public string Name { get; set; }

        [JsonProperty("weather_station.icao")]
        public string ICAO { get; set; }

        [JsonProperty("weather_station.iata")]
        public string IATA { get; set; }

        [JsonProperty("weather_station.synop")]
        public string SYNOP { get; set; }

        [JsonProperty("weather_station.lat")]
        public string Latitude { get; set; }

        [JsonProperty("weather_station.lon")]
        public string Longitude { get; set; }

        [JsonProperty("weather_station.elevation")]
        public float? Elevation { get; set; }
    }
}
