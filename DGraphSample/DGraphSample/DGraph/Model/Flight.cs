// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Newtonsoft.Json;

namespace DGraphSample.DGraph.Model
{
    public class Flight
    {
        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("flight_number")]
        public string FlightNumber { get; set; }

        [JsonProperty("tail_number")]
        public string TailNumber { get; set; }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("origin_airport")]
        public string OriginAirport { get; set; }

        [JsonProperty("destination_airport")]
        public string DestinationAirport { get; set; }

        [JsonProperty("flight_date")]
        public DateTime FlightDate { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("day_of_month")]
        public int DayOfMonth { get; set; }

        [JsonProperty("day_of_week")]
        public int DayOfWeek { get; set; }

        [JsonProperty("departure_delay")]
        public int? DepartureDelay { get; set; }

        [JsonProperty("arrival_delay")]
        public int? ArrivalDelay { get; set; }

        [JsonProperty("distance")]
        public float? Distance { get; set; }

        [JsonProperty("carrier_delay")]
        public int? CarrierDelay { get; set; }

        [JsonProperty("weather_delay")]
        public int? WeatherDelay { get; set; }

        [JsonProperty("nas_delay")]
        public int? NasDelay { get; set; }

        [JsonProperty("security_delay")]
        public int? SecurityDelay { get; set; }

        [JsonProperty("late_aircraft_delay")]
        public int? LateAircraftDelay { get; set; }

        [JsonProperty("cancellation_code")]
        public string CancellationCode { get; set; }
    }
}
