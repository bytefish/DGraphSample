// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Api;
using DGraphSample.Api.Client;
using DGraphSample.DGraph.Dto;
using DGraphSample.DGraph.Model;
using DGraphSample.DGraph.Utils;

namespace DGraphSample.DGraph.Processors
{
    public class AirportBatchProcessor
    {
        private readonly DGraphClient client;

        public AirportBatchProcessor(DGraphClient client)
        {
            this.client = client;
        }

        public async Task<Assigned> ProcessAsync(IList<AirportDto> airports, CancellationToken cancellationToken)
        {
            var transaction = client.NewTxn();

            // Get the Mutation:
            var mutation = GetMutation(airports);

            // Commit instantly:
            mutation.CommitNow = true;

            return await transaction.MutateAsync(mutation, cancellationToken);
        }

        private static Mutation GetMutation(IList<AirportDto> airports)
        {
            Mutation mutation = new Mutation();

            foreach (var airport in airports)
            {
                var nquads = Convert(airport);

                mutation.Set.AddRange(nquads);
            }

            return mutation;
        }

        private static List<NQuad> Convert(AirportDto airport)
        {
            return new NQuadBuilder(airport.Name)
                .Add(Constants.Predicates.Type, Constants.Types.Airport)
                .Add(Constants.Predicates.Name, airport.Name)
                .Add(Constants.Predicates.Abbr, airport.Abbr)
                .Add(Constants.Predicates.City, airport.City)
                .Add(Constants.Predicates.State, airport.State)
                .Add(Constants.Predicates.Country, airport.Country)
                .Build();
        }
    }
}