// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.DGraph.Dto;
using DGraphSample.DGraph.Resolvers;
using DGraphSample.DGraph.Utils;
using TinyDgraphClient.Generated;
using TinyDgraphClient.Client;

namespace DGraphSample.DGraph.Processors
{
    public class FlightBatchProcessor
    {
        private readonly DGraphClient client;
        private readonly AirportResolver airportResolver;
        private readonly CarrierResolver carrierResolver;

        public FlightBatchProcessor(DGraphClient client, AirportResolver airportResolver, CarrierResolver carrierResolver)
        {
            this.client = client;
            this.airportResolver = airportResolver;
            this.carrierResolver = carrierResolver;
        }

        public async Task<Assigned> ProcessAsync(IList<FlightDto> flights, CancellationToken cancellationToken)
        {
            var transaction = client.NewTxn();

            // Get the Mutation:
            var mutation = GetMutation(flights);

            // Commit instantly:
            mutation.CommitNow = true;

            return await transaction.MutateAsync(mutation, cancellationToken);
        }

        private Mutation GetMutation(IList<FlightDto> flights)
        {
            Mutation mutation = new Mutation();

            for (int pos = 0; pos < flights.Count; pos++)
            {
                var flight = flights[pos];
                var nquads = Convert(flight, pos);

                mutation.Set.AddRange(nquads);
            }

            return mutation;
        }

        private List<NQuad> Convert(FlightDto flight, int pos)
        {
            // We use flight_{position} as the Subjects ID. That's because 
            // we don't want to put too much logic in here to build a very 
            // unique subject id:
            var builder = new NQuadBuilder($"_:flight_{pos}")
                .Add(Constants.Predicates.Type, Constants.Types.Flight)
                .Add(Constants.Predicates.FlightNumber, flight.FlightNumber)
                .Add(Constants.Predicates.FlightTailNumber, flight.TailNumber)
                .Add(Constants.Predicates.FlightDate, flight.FlightDate)
                .Add(Constants.Predicates.FlightDayOfWeek, flight.DayOfWeek)
                .Add(Constants.Predicates.FlightDayOfMonth, flight.DayOfMonth)
                .Add(Constants.Predicates.FlightMonth, flight.Month)
                .Add(Constants.Predicates.FlightYear, flight.Year);

            // Set Airports:
            if (airportResolver.TryGetByAirportId(flight.OriginAirport, out string originAirportUid))
            {
                builder.AddEdge(Constants.Predicates.HasOriginAirport, originAirportUid);
            }

            if (airportResolver.TryGetByAirportId(flight.DestinationAirport, out string destinationAirportUid))
            {
                builder.AddEdge(Constants.Predicates.HasDestinationAirport, destinationAirportUid);
            }

            // Set Carrier:
            if (carrierResolver.TryGetByCode(flight.Carrier, out string carrierUid))
            {
                builder.AddEdge(Constants.Predicates.HasCarrier, carrierUid);
            }

            // Add Distance:
            if (flight.Distance.HasValue)
            {
                builder.Add(Constants.Predicates.FlightDistance, flight.Distance.Value);
            }

            // Add CancellationCode:
            if (!string.IsNullOrWhiteSpace(flight.CancellationCode))
            {
                builder.Add(Constants.Predicates.FlightCancellationCode, flight.CancellationCode);
            }

            // Add Delays:
            if (flight.ArrivalDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightArrivalDelay, flight.ArrivalDelay.Value);
            }

            if (flight.CarrierDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightCarrierDelay, flight.CarrierDelay.Value);
            }

            if (flight.DepartureDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightDepartureDelay, flight.DepartureDelay.Value);
            }

            if (flight.LateAircraftDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightLateAircraftDelay, flight.LateAircraftDelay.Value);
            }

            if (flight.NasDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightNasDelay, flight.NasDelay.Value);
            }

            if (flight.SecurityDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightSecurityDelay, flight.SecurityDelay.Value);
            }

            if (flight.WeatherDelay.HasValue)
            {
                builder.Add(Constants.Predicates.FlightWeatherDelay, flight.WeatherDelay.Value);
            }

            return builder.Build();
        }
    }
}