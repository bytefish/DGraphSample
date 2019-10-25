// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Faa.Model;
using TinyCsvParser.Mapping;

namespace DGraphSample.Csv.Faa.Mapper
{
    public class FaaAircraftMapper : CsvMapping<FaaAircraft>
    {
        public FaaAircraftMapper()
        {
            MapProperty(0, x => x.N_Number);
            MapProperty(1, x => x.SerialNumber);
            MapProperty(2, x => x.UniqueId);
            MapProperty(3, x => x.AircraftManufacturer);
            MapProperty(4, x => x.AircraftModel);
            MapProperty(5, x => x.AircraftSeats);
            MapProperty(6, x => x.EngineManufacturer);
            MapProperty(7, x => x.EngineModel);
            MapProperty(8, x => x.EngineHorsepower);
            MapProperty(9, x => x.EngineThrust);
        }
    }
}
