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

        [JsonProperty("flight.flight_number")]
        public string FlightNumber { get; set; }

        [JsonProperty("flight.tail_number")]
        public string TailNumber { get; set; }

        [JsonProperty("flight.carrier")]
        public string Carrier { get; set; }

        [JsonProperty("flight.origin_airport")]
        public string OriginAirport { get; set; }

        [JsonProperty("flight.destination_airport")]
        public string DestinationAirport { get; set; }

        [JsonProperty("flight.flight_date")]
        public DateTime FlightDate { get; set; }

        [JsonProperty("flight.year")]
        public int Year { get; set; }

        [JsonProperty("flight.month")]
        public int Month { get; set; }

        [JsonProperty("flight.day_of_month")]
        public int DayOfMonth { get; set; }

        [JsonProperty("flight.day_of_week")]
        public int DayOfWeek { get; set; }

        [JsonProperty("flight.departure_delay")]
        public int? DepartureDelay { get; set; }

        [JsonProperty("flight.arrival_delay")]
        public int? ArrivalDelay { get; set; }

        [JsonProperty("flight.distance")]
        public float? Distance { get; set; }

        [JsonProperty("flight.carrier_delay")]
        public int? CarrierDelay { get; set; }

        [JsonProperty("flight.weather_delay")]
        public int? WeatherDelay { get; set; }

        [JsonProperty("flight.nas_delay")]
        public int? NasDelay { get; set; }

        [JsonProperty("flight.security_delay")]
        public int? SecurityDelay { get; set; }

        [JsonProperty("flight.late_aircraft_delay")]
        public int? LateAircraftDelay { get; set; }

        [JsonProperty("flight.cancellation_code")]
        public string CancellationCode { get; set; }
    }
}
