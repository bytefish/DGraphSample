// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DGraphSample.DGraph.Model
{
    public class FlightInformation
    {
        public string FlightNumber { get; set; }

        public string TailNumber { get; set; }

        public string Carrier { get; set; }

        public string OriginAirport { get; set; }

        public string DestinationAirport { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int DayOfMonth { get; set; }

        public int DayOfWeek { get; set; }

        public IList<Delay> Delays { get; set; }

        public int? TaxiOut { get; set; }

        public int? DepartureDelay { get; set; }

        public int? TaxiIn { get; set; }

        public int? ArrivalDelay { get; set; }

        public string CancellationCode { get; set; }
    }
}
