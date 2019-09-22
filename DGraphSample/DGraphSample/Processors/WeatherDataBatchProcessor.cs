// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Dto;
using DGraphSample.Resolvers;
using DGraphSample.Utils;
using TinyDgraphClient.Client;
using TinyDgraphClient.Generated;

namespace DGraphSample.Processors
{
    public class WeatherDataBatchProcessor
    {
        private readonly DGraphClient client;
        private readonly WeatherStationResolver weatherStationResolver;

        public WeatherDataBatchProcessor(DGraphClient client, WeatherStationResolver weatherStationResolver)
        {
            this.client = client;
            this.weatherStationResolver = weatherStationResolver;
        }

        public async Task<Assigned> ProcessAsync(IList<WeatherDataDto> measurements, CancellationToken cancellationToken)
        {
            var transaction = client.NewTxn();

            // Get the Mutation:
            Mutation mutation = GetMutation(measurements);

            // Commit instantly:
            mutation.CommitNow = true;

            return await transaction.MutateAsync(mutation, cancellationToken);
        }

        private Mutation GetMutation(IList<WeatherDataDto> measurements)
        {
            Mutation mutation = new Mutation();

            for(int pos = 0; pos < measurements.Count; pos++)
            {
                var measurement = measurements[pos];

                var nquads = Convert(measurement, pos);

                mutation.Set.AddRange(nquads);
            }

            return mutation;
        }

        private List<NQuad> Convert(WeatherDataDto measurement, int pos)
        {
            var builder = new NQuadBuilder($"_:weather_data_{pos}")
                .Add(Constants.Predicates.Type, Constants.Types.WeatherData)
                .Add(Constants.Predicates.WeatherDataStation, measurement.station)
                .Add(Constants.Predicates.WeatherDataAlti, measurement.alti)
                .Add(Constants.Predicates.WeatherDataDrct, measurement.drct)
                .Add(Constants.Predicates.WeatherDataDwpc, measurement.dwpc)
                .Add(Constants.Predicates.WeatherDataDwpf, measurement.dwpf)
                .Add(Constants.Predicates.WeatherDataFeelc, measurement.feelc)
                .Add(Constants.Predicates.WeatherDataFeelf, measurement.feelf)
                .Add(Constants.Predicates.WeatherDataGust, measurement.gust)
                .Add(Constants.Predicates.WeatherDataIceAccretion1hr, measurement.ice_accretion_1hr)
                .Add(Constants.Predicates.WeatherDataIceAccretion3hr, measurement.ice_accretion_3hr)
                .Add(Constants.Predicates.WeatherDataIceAccretion6hr, measurement.ice_accretion_6hr)
                .Add(Constants.Predicates.WeatherDataLatitude, measurement.lat)
                .Add(Constants.Predicates.WeatherDataLongitude, measurement.lon)
                .Add(Constants.Predicates.WeatherDataMetar, measurement.metar)
                .Add(Constants.Predicates.WeatherDataMslp, measurement.mslp)
                .Add(Constants.Predicates.WeatherDataP01i, measurement.p01i)
                .Add(Constants.Predicates.WeatherDataPeakWindDrct, measurement.peak_wind_drct)
                .Add(Constants.Predicates.WeatherDataPeakWindGust, measurement.peak_wind_gust)
                .Add(Constants.Predicates.WeatherDataPeakWindTime_hh, measurement.peak_wind_time_hh)
                .Add(Constants.Predicates.WeatherDataPeakWindTime_MM, measurement.peak_wind_time_MM)
                .Add(Constants.Predicates.WeatherDataRelh, measurement.relh)
                .Add(Constants.Predicates.WeatherDataSknt, measurement.sknt)
                .Add(Constants.Predicates.WeatherDataSkyc1, measurement.skyc1)
                .Add(Constants.Predicates.WeatherDataSkyc2, measurement.skyc2)
                .Add(Constants.Predicates.WeatherDataSkyc3, measurement.skyc3)
                .Add(Constants.Predicates.WeatherDataSkyc4, measurement.skyc4)
                .Add(Constants.Predicates.WeatherDataSkyl1, measurement.skyl1)
                .Add(Constants.Predicates.WeatherDataSkyl2, measurement.skyl2)
                .Add(Constants.Predicates.WeatherDataSkyl3, measurement.skyl3)
                .Add(Constants.Predicates.WeatherDataSkyl4, measurement.skyl4)
                .Add(Constants.Predicates.WeatherDataTimestamp, measurement.valid)
                .Add(Constants.Predicates.WeatherDataTmpc, measurement.tmpc)
                .Add(Constants.Predicates.WeatherDataTmpf, measurement.tmpf)
                .Add(Constants.Predicates.WeatherDataVsbyKm, measurement.vsby_km)
                .Add(Constants.Predicates.WeatherDataVsbyMi, measurement.vsby_mi)
                .Add(Constants.Predicates.WeatherDataWxcodes, measurement.wxcodes);

            if(weatherStationResolver.TryGetByICAO(measurement.station, out string uid))
            {
                builder.AddEdge(Constants.Predicates.HasWeatherStation, uid);
            }

            return builder.Build();
        }
    }
}