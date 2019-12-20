// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Xml;

namespace DGraphSample.Exporter.Dto
{
    public class AircraftDto
    {
        public string NodeId { get; set; }

        public string N_Number { get; set; }

        public string SerialNumber { get; set; }

        public string UniqueId { get; set; }

        public string AircraftManufacturer { get; set; }

        public string AircraftModel { get; set; }

        public string AircraftSeats { get; set; }

        public string EngineManufacturer { get; set; }

        public string EngineModel { get; set; }

        public float? EngineHorsepower { get; set; }

        public float? EngineThrust { get; set; }
    }
}
