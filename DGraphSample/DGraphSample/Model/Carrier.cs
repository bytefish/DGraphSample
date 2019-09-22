// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json;

namespace DGraphSample.Model
{
    public class CarrierList
    {
        [JsonProperty("carriers")]
        public Carrier[] Carriers { get; set; }
    }

    public class Carrier
    {
        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("airport.code")]
        public string Code { get; set; }

        [JsonProperty("airport.description")]
        public string Description { get; set; }
    }
}
