// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Asos.Converter;
using DGraphSample.Csv.Asos.Model;
using System;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace DGraphSample.Csv.Asos.Mapper
{
    public class AsosMeasurementMapper : CsvMapping<AsosMeasurement>
    {
        public AsosMeasurementMapper()
        {
            MapProperty(0, x => x.station);
            MapProperty(1, x => x.valid, M(new NullableDateTimeConverter()));
            MapProperty(2, x => x.lon, M(new NullableSingleConverter()));
            MapProperty(3, x => x.lat, M(new NullableSingleConverter()));
            MapProperty(4, x => x.tmpf, M(new NullableSingleConverter()));
            MapProperty(5, x => x.dwpf, M(new NullableSingleConverter()));
            MapProperty(6, x => x.relh, M(new NullableSingleConverter()));
            MapProperty(7, x => x.drct, M(new NullableSingleConverter()));
            MapProperty(8, x => x.sknt, M(new NullableSingleConverter()));
            MapProperty(9, x => x.p01i, M(new NullableSingleConverter()));
            MapProperty(10, x => x.alti, M(new NullableSingleConverter()));
            MapProperty(11, x => x.mslp, M(new NullableSingleConverter()));
            MapProperty(12, x => x.vsby, M(new NullableSingleConverter()));
            MapProperty(13, x => x.gust, M(new NullableSingleConverter()));
            MapProperty(14, x => x.skyc1, M(new StringConverter()));
            MapProperty(15, x => x.skyc2, M(new StringConverter()));
            MapProperty(16, x => x.skyc3, M(new StringConverter()));
            MapProperty(17, x => x.skyc4, M(new StringConverter()));
            MapProperty(18, x => x.skyl1, M(new NullableSingleConverter()));
            MapProperty(19, x => x.skyl2, M(new NullableSingleConverter()));
            MapProperty(20, x => x.skyl3, M(new NullableSingleConverter()));
            MapProperty(21, x => x.skyl4, M(new NullableSingleConverter()));
            MapProperty(22, x => x.wxcodes, M(new StringConverter()));
            MapProperty(23, x => x.ice_accretion_1hr, M(new NullableSingleConverter()));
            MapProperty(24, x => x.ice_accretion_3hr, M(new NullableSingleConverter()));
            MapProperty(25, x => x.ice_accretion_6hr, M(new NullableSingleConverter()));
            MapProperty(26, x => x.peak_wind_gust, M(new NullableSingleConverter()));
            MapProperty(27, x => x.peak_wind_drct, M(new NullableSingleConverter()));
            MapProperty(28, x => x.peak_wind_time, M(new AsosTimeConverter()));
            MapProperty(29, x => x.feel, M(new NullableSingleConverter()));
            MapProperty(30, x => x.metar, M(new StringConverter()));
        }

        private static MissingValuesConverter<TTargetType> M<TTargetType>(ITypeConverter<TTargetType> converter)
        {
            return new MissingValuesConverter<TTargetType>("M", converter);
        }
    }
}
