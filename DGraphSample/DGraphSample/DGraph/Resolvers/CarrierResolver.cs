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
    public class CarrierResolver
    {
        private readonly Dictionary<string, string> lookup;

        public CarrierResolver(Carrier[] carriers)
        {
            this.lookup = carriers.ToDictionary(x => x.Code, x => x.UID);
        }

        public bool TryGetByCode(string code, out string uid)
        {
            return lookup.TryGetValue(code, out uid);
        }

        public static async Task<CarrierResolver> CreateResolverAsync(DGraphClient client)
        {
            var transaction = client.NewReadOnlyTxn();

            // Query DGraph:
            var response = await transaction.QueryAsync(Resources.DGraphQueries.GetAllCarriers, CancellationToken.None);

            // Deserialize the Result:
            var carriers = ProtobufUtils.Deserialize<CarrierList>(response.Json);

            return new CarrierResolver(carriers.Carriers);
        }
    }
}
