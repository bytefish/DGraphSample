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

public class AirportBatchProcessor
{
    private readonly DGraphClient client;

    public AirportBatchProcessor(DGraphClient client)
    {
        this.client = client;
    }

    public async Task<Assigned> ProcessAsync(List<Airport> airports, CancellationToken cancellationToken)
    {
        var transaction = client.NewTxn();
        
        // Get the Mutation:
        var mutation = GetMutation(airports);

        // Commit instantly:
        mutation.CommitNow = true;
        
        return await transaction.MutateAsync(mutation, cancellationToken);        
    }

    private static Mutation GetMutation(List<Airport> airports)
    {
        Mutation mutation = new Mutation();

        foreach (var airport in airports)
        {
            var nquads = Convert(airport);

            mutation.Set.AddRange(nquads);
        }

        return mutation;
    }

    private static List<NQuad> Convert(Airport airport)
    {
        return new NQuadBuilder(airport.Name)
            .Add(Constants.Predicates.Type, Constants.Types.Airport)
            .Add(Constants.Predicates.Name, airport.Name)
            .Add(Constants.Predicates.City, airport.City)
            .Add(Constants.Predicates.State, airport.State)
            .Add(Constants.Predicates.Country, airport.Country)
            .Build();
    }
}