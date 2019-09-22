// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace DGraphSample.DGraph.Model
{
    public class AirportList
    {
        [JsonProperty("airports")]
        public Airport[] Airports { get; set; }
    }

    public class Airport
    {
        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("airport.iata")]
        public string AirportId { get; set; }

        [JsonProperty("airport.name")]
        public string Name { get; set; }

        [JsonProperty("airport.abbr")]
        public string Abbr { get; set; }

        [JsonProperty("airport.city")]
        public string City { get; set; }

        [JsonProperty("airport.state")]
        public string State { get; set; }

        [JsonProperty("airport.country")]
        public string Country { get; set; }
    }
}