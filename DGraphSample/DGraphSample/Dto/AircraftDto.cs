using System;
using System.Collections.Generic;
using System.Text;

namespace DGraphSample.Dto
{
    public class AircraftDto
    {
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
