// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Api.Client;
using DGraphSample.DGraph.Model;
using DGraphSample.DGraph.Utils;

namespace DGraphSample.DGraph.Resolvers
{
    public class AirportResolver
    {
        private readonly Dictionary<string, string> lookup;

        public AirportResolver(Airport[] airports)
        {
            this.lookup = airports
                .GroupBy(x => x.AirportId)
                .Select(x => x.First())
                .ToDictionary(x => x.AirportId, x => x.UID);
        }

        public bool TryGetByAirportId(string name, out string uid)
        {
            return lookup.TryGetValue(name, out uid);
        }

        public static async Task<AirportResolver> CreateResolverAsync(DGraphClient client)
        {
            var transaction = client.NewReadOnlyTxn();

            // Query DGraph:
            var response = await transaction.QueryAsync(Resources.DGraphQueries.GetAllAirports, CancellationToken.None);

            // Deserialize the Result:
            var airports = ProtobufUtils.Deserialize<AirportList>(response.Json);

            return new AirportResolver(airports.Airports);
        }
    }
}
