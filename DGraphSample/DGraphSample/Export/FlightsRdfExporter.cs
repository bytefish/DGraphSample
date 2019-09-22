using DGraphSample.DGraph;
using DGraphSample.DGraph.Dto;
using DGraphSample.DGraph.Model;
using DGraphSample.DGraph.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TinyDgraphClient.Generated;

namespace DGraphSample.Export
{
    public static class FlightsRdfExporter
    {
        public static IEnumerable<string> ConvertToRdf(string identifier, CarrierDto carrier)
        {
            var nquads = ConvertToNquad(identifier, carrier);

            foreach (var nquad in nquads)
            {
                if (TryConvertToRdf(nquad, out string result))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<string> ConvertToRdf(string identifier, AirportDto airport)
        {
            var nquads = ConvertToNquad(identifier, airport);

            foreach (var nquad in nquads)
            {
                if (TryConvertToRdf(nquad, out string result))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<string> ConvertToRdf(string identifier, FlightDto flight)
        {
            var nquads = ConvertToNquad(identifier, flight);

            foreach (var nquad in nquads)
            {
                if (TryConvertToRdf(nquad, out string result))
                {
                    yield return result;
                }
            }
        }

        private static bool TryConvertToRdf(NQuad nquad, out string result)
        {
            result = string.Empty;

            if (!string.IsNullOrWhiteSpace(nquad.ObjectId))
            {
                result = $"<{nquad.Subject}> <{nquad.Predicate}> <{nquad.ObjectId}> .";

                return true;
            }

            if (nquad.ObjectValue == null)
            {
                return false;
            }

            if (TryGetFormattedValue(nquad.ObjectValue, out string valueAsString))
            {
                result = $"<{nquad.Subject}> <{nquad.Predicate}> {valueAsString} .";
            }

            return false;
        }

        private static bool TryGetFormattedValue(Value value, out string result)
        {
            result = string.Empty;

            if (value == null)
            {
                return false;
            }

            switch (value.ValCase)
            {
                case Value.ValOneofCase.IntVal:
                    result = value.IntVal.ToString(CultureInfo.InvariantCulture);
                    break;
                case Value.ValOneofCase.DoubleVal:
                    result = value.DoubleVal.ToString(CultureInfo.InvariantCulture);
                    break;
                case Value.ValOneofCase.StrVal:
                    result = $"\"{value.StrVal}\"";
                    break;
                case Value.ValOneofCase.BoolVal:
                    result = value.BoolVal ? "true" : "false";
                    break;
                case Value.ValOneofCase.DatetimeVal:
                    result = value.DatetimeVal.ToString();
                    break;
                case Value.ValOneofCase.GeoVal:
                    result = value.GeoVal.ToString();
                    break;
                default:
                    return false;
            }

            return true;
        }

        private static List<NQuad> ConvertToNquad(string identifier, AirportDto airport)
        {
            return new NQuadBuilder($"airport_{identifier}")
                .Add(Constants.Predicates.Type, Constants.Types.Airport)
                .Add(Constants.Predicates.AirportIata, airport.Iata)
                .Add(Constants.Predicates.AirportName, airport.Name)
                .Add(Constants.Predicates.AirportAbbr, airport.Abbr)
                .Add(Constants.Predicates.AirportCity, airport.City)
                .Add(Constants.Predicates.AirportState, airport.State)
                .Add(Constants.Predicates.AirportCountry, airport.Country)
                .Build();
        }

        private static List<NQuad> ConvertToNquad(string identifier, CarrierDto carrier)
        {
            return new NQuadBuilder($"carrier_{identifier}")
                .Add(Constants.Predicates.Type, Constants.Types.Carrier)
                .Add(Constants.Predicates.CarrierCode, carrier.Code)
                .Add(Constants.Predicates.AirportName, carrier.Description)
                .Build();
        }

        private static List<NQuad> ConvertToNquad(string identifier, FlightDto flight)
        {
            // We use flight_{identifier} as the Subjects ID. That's because 
            // we don't want to put too much logic in here to build a very 
            // unique subject id:
            var builder = new NQuadBuilder($"flight_{identifier}")
                .Add(Constants.Predicates.Type, Constants.Types.Flight)
                .Add(Constants.Predicates.FlightNumber, flight.FlightNumber)
                .Add(Constants.Predicates.FlightTailNumber, flight.TailNumber)
                .Add(Constants.Predicates.FlightDate, flight.FlightDate)
                .Add(Constants.Predicates.FlightDayOfWeek, flight.DayOfWeek)
                .Add(Constants.Predicates.FlightDayOfMonth, flight.DayOfMonth)
                .Add(Constants.Predicates.FlightMonth, flight.Month)
                .Add(Constants.Predicates.FlightYear, flight.Year);

            // Set Airports:
            builder.AddEdge(Constants.Predicates.HasOriginAirport, $"airport_{flight.OriginAirport}");
            builder.AddEdge(Constants.Predicates.HasDestinationAirport, $"airport_{flight.DestinationAirport}");

            // Set Carrier:
            builder.AddEdge(Constants.Predicates.HasCarrier, $"carrier_{flight.Carrier}");

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
