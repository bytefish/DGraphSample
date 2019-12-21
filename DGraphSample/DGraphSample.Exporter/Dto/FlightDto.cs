// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DGraphSample.Exporter.Dto
{
    public class FlightDto
    {
        public string NodeId { get; set; }

        public string FlightNumber { get; set; }

        public string TailNumber { get; set; }

        public string Carrier { get; set; }

        public string OriginAirport { get; set; }

        public string DestinationAirport { get; set; }

        public DateTime FlightDate { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int DayOfMonth { get; set; }

        public int DayOfWeek { get; set; }

        public int? DepartureDelay { get; set; }

        public int? ArrivalDelay { get; set; }

        public float? Distance { get; set; }

        public int? CarrierDelay { get; set; }

        public int? WeatherDelay { get; set; }

        public int? NasDelay { get; set; }

        public int? SecurityDelay { get; set; }

        public int? LateAircraftDelay { get; set; }

        public string CancellationCode { get; set; }

        public DateTime? ActualDepartureTime { get; set; }

        public DateTime? ScheduledDepartureTime { get; set; }

    }
}
