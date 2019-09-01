// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Api;
using DGraphSample.Api.Client;
using DGraphSample.DGraph;
using DGraphSample.DGraph.BatchProcessor;
using DGraphSample.DGraph.Model;

public class CarrierBatchProcessor
{
    private readonly DGraphClient client;

    public CarrierBatchProcessor(DGraphClient client)
    {
        this.client = client;
    }

    public async Task<Assigned> ProcessAsync(List<Carrier> carriers, CancellationToken cancellationToken)
    {
        var transaction = client.NewTxn();
        
        // Get the Mutation:
        var mutation = GetMutation(carriers);

        // Commit instantly:
        mutation.CommitNow = true;
        
        return await transaction.MutateAsync(mutation, cancellationToken);        
    }

    private static Mutation GetMutation(List<Carrier> carriers)
    {
        Mutation mutation = new Mutation();

        foreach (var carrier in carriers)
        {
            var nquads = Convert(carrier);

            mutation.Set.AddRange(nquads);
        }

        return mutation;
    }

    private static List<NQuad> Convert(Carrier carrier)
    {
        return new NQuadBuilder(carrier.Code)
            .Add(Constants.Predicates.Type, Constants.Types.Carrier)
            .Add(Constants.Predicates.Code, carrier.Code)
            .Build();
    }
}