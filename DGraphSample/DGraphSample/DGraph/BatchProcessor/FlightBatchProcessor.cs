// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Api;
using DGraphSample.Api.Client;
using DGraphSample.DGraph;
using DGraphSample.DGraph.BatchProcessor;
using DGraphSample.DGraph.Model;

public class FlightBatchProcessor
{
    private readonly DGraphClient client;

    public FlightBatchProcessor(DGraphClient client)
    {
        this.client = client;   
    }

    public async Task<Assigned> ProcessAsync(List<Flight> flights, CancellationToken cancellationToken)
    {
        var transaction = client.NewTxn();
        
        // Get the Mutation:
        var mutation = GetMutation(flights);

        // Commit instantly:
        mutation.CommitNow = true;
        
        return await transaction.MutateAsync(mutation, cancellationToken);        
    }

    private static Mutation GetMutation(List<Flight> flights)
    {
        Mutation mutation = new Mutation();

        for (int pos = 0; pos < flights.Count; pos++)
        {
            var flight = flights[pos];
            var nquads = Convert(pos, flight);

            mutation.Set.AddRange(nquads);
        }

        return mutation;
    }

    private static List<NQuad> Convert(int pos, Flight flight)
    {

        var builder = new NQuadBuilder($"flight_{pos}")
            .Add(Constants.Predicates.Type, Constants.Types.Flight)
            .Add(Constants.Predicates.FlightNumber, flight.FlightNumber)
            .Add(Constants.Predicates.TailNumber, flight.TailNumber)
            .Add(Constants.Predicates.FlightDate, flight.FlightDate)
            .Add(Constants.Predicates.DayOfWeek, flight.DayOfWeek)
            .Add(Constants.Predicates.DayOfMonth, flight.DayOfMonth)
            .Add(Constants.Predicates.Month, flight.Month)
            .Add(Constants.Predicates.Year, flight.Year);
        
        // Add Distance:
        if (flight.Distance.HasValue)
        {
            builder.Add(Constants.Predicates.Distance, flight.CarrierDelay.Value);
        }

        // Add CancellationCode:
        if (!string.IsNullOrWhiteSpace(flight.CancellationCode))
        {
            builder.Add(Constants.Predicates.CancellationCode, flight.CancellationCode);
        }

        // Add Delays:
        if (flight.ArrivalDelay.HasValue)
        {
            builder.Add(Constants.Predicates.ArrivalDelay, flight.ArrivalDelay.Value);
        }

        if (flight.CarrierDelay.HasValue)
        {
            builder.Add(Constants.Predicates.CarrierDelay, flight.CarrierDelay.Value);
        }

        if (flight.DepartureDelay.HasValue)
        {
            builder.Add(Constants.Predicates.DepartureDelay, flight.DepartureDelay.Value);
        }

        if (flight.LateAircraftDelay.HasValue)
        {
            builder.Add(Constants.Predicates.LateAircraftDelay, flight.LateAircraftDelay.Value);
        }

        if (flight.NasDelay.HasValue)
        {
            builder.Add(Constants.Predicates.NasDelay, flight.NasDelay.Value);
        }

        if (flight.SecurityDelay.HasValue)
        {
            builder.Add(Constants.Predicates.SecurityDelay, flight.SecurityDelay.Value);
        }

        if (flight.WeatherDelay.HasValue)
        {
            builder.Add(Constants.Predicates.WeatherDelay, flight.WeatherDelay.Value);
        }

        return builder.Build();
    }
}