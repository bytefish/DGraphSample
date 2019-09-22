// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Dto;
using DGraphSample.Resolvers;
using DGraphSample.Utils;
using TinyDgraphClient.Generated;
using TinyDgraphClient.Client;

namespace DGraphSample.Processors
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
                .Add(Constants.Predicates.FlightYear, flight.Year)
                .Add(Constants.Predicates.FlightDistance, flight.Distance)
                .Add(Constants.Predicates.FlightArrivalDelay, flight.ArrivalDelay)
                .Add(Constants.Predicates.FlightCarrierDelay, flight.CarrierDelay)
                .Add(Constants.Predicates.FlightDepartureDelay, flight.DepartureDelay)
                .Add(Constants.Predicates.FlightLateAircraftDelay, flight.LateAircraftDelay)
                .Add(Constants.Predicates.FlightNasDelay, flight.NasDelay)
                .Add(Constants.Predicates.FlightSecurityDelay, flight.SecurityDelay)
                .Add(Constants.Predicates.FlightWeatherDelay, flight.WeatherDelay.Value);

            // Only add CancellationCode if not empty:
            if (!string.IsNullOrWhiteSpace(flight.CancellationCode))
            {
                builder.Add(Constants.Predicates.FlightCancellationCode, flight.CancellationCode);
            }

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

            return builder.Build();
        }
    }
}