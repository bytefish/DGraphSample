// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Model;
using DGraphSample.Queries;
using DGraphSample.Utils;
using TinyDgraphClient.Client;

namespace DGraphSample.Resolvers
{
    public class WeatherStationResolver
    {
        private readonly Dictionary<string, string> lookup;

        public WeatherStationResolver(WeatherStation[] stations)
        {
            this.lookup = stations
                .GroupBy(x => x.ICAO)
                .Select(x => x.First())
                .ToDictionary(x => x.ICAO, x => x.UID);
        }

        public bool TryGetByICAO(string icao, out string uid)
        {
            return lookup.TryGetValue(icao, out uid);
        }

        public static async Task<WeatherStationResolver> CreateResolverAsync(DGraphClient client)
        {
            var transaction = client.NewReadOnlyTxn();

            // Query DGraph:
            var response = await transaction.QueryAsync(Query.GetAllWeatherStations, CancellationToken.None);

            // Deserialize the Result:
            var result = ProtobufUtils.Deserialize<WeatherStationList>(response.Json);

            return new WeatherStationResolver(result.Stations);
        }
    }
}
