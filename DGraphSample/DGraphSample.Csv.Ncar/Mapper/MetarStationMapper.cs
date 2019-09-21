// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Ncar.Converter;
using DGraphSample.Csv.Ncar.Model;
using DGraphSample.Csv.Ncar.Model.Enums;
using TinyCsvParser.Mapping;

namespace DGraphSample.Csv.Ncar.Mapper
{
    public class MetarStationMapper : CsvMapping<MetarStation>
    {
        public MetarStationMapper()
        {
            MapProperty(0, x => x.CD);
            MapProperty(1, x => x.Station);
            MapProperty(2, x => x.ICAO);
            MapProperty(3, x => x.IATA);
            MapProperty(4, x => x.SYNOP);
            MapProperty(5, x => x.Latitude);
            MapProperty(6, x => x.Longitude);
            MapProperty(7, x => x.Elevation);
            MapProperty(8, x => x.M, new MetarEnumConverter<MetarFlagEnum>());
            MapProperty(9, x => x.N, new MetarEnumConverter<NexradFlagEnum>());
            MapProperty(10, x => x.V, new MetarEnumConverter<AviationFlagEnum>());
            MapProperty(11, x => x.U, new MetarEnumConverter<UpperAirFlagEnum>());
            MapProperty(12, x => x.A, new MetarEnumConverter<AutoFlagEnum>());
            MapProperty(13, x => x.C, new MetarEnumConverter<OfficeTypeFlagEnum>());
        }
    }
}
