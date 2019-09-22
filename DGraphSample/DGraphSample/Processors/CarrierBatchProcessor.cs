// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Model;
using DGraphSample.Utils;
using TinyDgraphClient.Generated;
using TinyDgraphClient.Client;
using DGraphSample.Dto;

namespace DGraphSample.Processors
{
    public class CarrierBatchProcessor
    {
        private readonly DGraphClient client;

        public CarrierBatchProcessor(DGraphClient client)
        {
            this.client = client;
        }

        public async Task<Assigned> ProcessAsync(IList<CarrierDto> carriers, CancellationToken cancellationToken)
        {
            var transaction = client.NewTxn();

            // Get the Mutation:
            var mutation = GetMutation(carriers);

            // Commit instantly:
            mutation.CommitNow = true;

            return await transaction.MutateAsync(mutation, cancellationToken);
        }

        private static Mutation GetMutation(IList<CarrierDto> carriers)
        {
            Mutation mutation = new Mutation();

            for (int pos = 0; pos < carriers.Count; pos++)
            {
                CarrierDto carrier = carriers[pos];

                var nquads = Convert(carrier, pos);

                mutation.Set.AddRange(nquads);
            }

            return mutation;
        }

        private static List<NQuad> Convert(CarrierDto carrier, int pos)
        {
            return new NQuadBuilder($"_:carrier_{pos}")
                .Add(Constants.Predicates.Type, Constants.Types.Carrier)
                .Add(Constants.Predicates.CarrierCode, carrier.Code)
                .Add(Constants.Predicates.CarrierDescription, carrier.Description)
                .Build();
        }
    }
}