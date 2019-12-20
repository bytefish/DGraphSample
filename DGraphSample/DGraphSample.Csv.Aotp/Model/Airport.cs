// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DGraphSample.Csv.Aotp.Model
{
    public class Airport
    {
        public string AirportId { get; set; }

        public string AirportIata { get; set; }

        public string AirportName { get; set; }

        public string AirportCityName { get; set; }

        public string AirportWac { get; set; }

        public string AirportCountryName { get; set; }

        public string AirportCountryCodeISO { get; set; }

        public string AirportStateName { get; set; }

        public string AirportStateCode { get; set; }

        public bool AirportIsLatest { get; set; }
    }
}
