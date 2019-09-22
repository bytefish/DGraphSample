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
    public class WeatherStationBatchProcessor
    {
        private readonly DGraphClient client;
        private readonly AirportResolver airportResolver;

        public WeatherStationBatchProcessor(DGraphClient client, AirportResolver airportResolver)
        {
            this.client = client;
            this.airportResolver = airportResolver;
        }

        public async Task<Assigned> ProcessAsync(IList<WeatherStationDto> stations, CancellationToken cancellationToken)
        {
            var transaction = client.NewTxn();

            // Get the Mutation:
            Mutation mutation = GetMutation(stations);

            // Commit instantly:
            mutation.CommitNow = true;

            return await transaction.MutateAsync(mutation, cancellationToken);
        }

        private Mutation GetMutation(IList<WeatherStationDto> stations)
        {
            Mutation mutation = new Mutation();

            for (int pos = 0; pos < stations.Count; pos++)
            {
                var station = stations[pos];

                var nquads = Convert(station, pos);

                mutation.Set.AddRange(nquads);
            }

            return mutation;
        }

        private List<NQuad> Convert(WeatherStationDto station, int pos)
        {
            var builder = new NQuadBuilder($"_:station_{pos}")
                .Add(Constants.Predicates.Type, Constants.Types.WeatherStation)
                .Add(Constants.Predicates.WeatherStationIata, station.IATA)
                .Add(Constants.Predicates.WeatherStationName, station.Station)
                .Add(Constants.Predicates.WeatherStationIcao, station.ICAO)
                .Add(Constants.Predicates.WeatherStationLat, station.Latitude)
                .Add(Constants.Predicates.WeatherStationLon, station.Longitude)
                .Add(Constants.Predicates.WeatherStationSynop, station.SYNOP)
                .Add(Constants.Predicates.WeatherStationElevation, station.Elevation.Value);

            if(airportResolver.TryGetByAirportId(station.IATA, out string uid))
            {
                builder.AddEdge(uid, Constants.Predicates.HasWeatherStation, $"_:station_{pos}");
            }

            return builder.Build();
        }
    }
}