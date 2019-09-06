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

        [JsonProperty("airport_id")]
        public string AirportId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("abbr")]
        public string Abbr { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}