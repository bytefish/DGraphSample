using DGraphSample.Dto;
using DGraphSample.Utils;
using System.Collections.Generic;
using System.Globalization;
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

        public static IEnumerable<string> ConvertToRdf(string identifier, WeatherStationDto station)
        {
            var nquads = ConvertToNquad(identifier, station);

            foreach (var nquad in nquads)
            {
                if (TryConvertToRdf(nquad, out string result))
                {
                    yield return result;
                }
            }
        }

        public static IEnumerable<string> ConvertToRdf(string identifier, WeatherDataDto weatherData)
        {
            var nquads = ConvertToNquad(identifier, weatherData);

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
            return new NQuadBuilder($"_:airport_{identifier}")
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
            return new NQuadBuilder($"_:carrier_{identifier}")
                .Add(Constants.Predicates.Type, Constants.Types.Carrier)
                .Add(Constants.Predicates.CarrierCode, carrier.Code)
                .Add(Constants.Predicates.AirportName, carrier.Description)
                .Build();
        }

        private static List<NQuad> ConvertToNquad(string identifier, WeatherStationDto station)
        {
            var builder = new NQuadBuilder($"_:weather_station_{identifier}")
                .Add(Constants.Predicates.Type, Constants.Types.WeatherStation)
                .Add(Constants.Predicates.WeatherStationIata, station.IATA)
                .Add(Constants.Predicates.WeatherStationName, station.Station)
                .Add(Constants.Predicates.WeatherStationIcao, station.ICAO)
                .Add(Constants.Predicates.WeatherStationLat, station.Latitude)
                .Add(Constants.Predicates.WeatherStationLon, station.Longitude)
                .Add(Constants.Predicates.WeatherStationSynop, station.SYNOP)
                .Add(Constants.Predicates.WeatherStationElevation, station.Elevation);

            // TODO Here we need a Lookup for the Airport IATA, because we can't be sure there will be blank node for it:
            builder.AddEdge($"_:airport_{station.IATA}", Constants.Predicates.HasWeatherStation, $"_:weather_station_{identifier}");

            return builder.Build();
        }

        private static List<NQuad> ConvertToNquad(string identifier, WeatherDataDto measurement)
        {
            var builder = new NQuadBuilder($"_:weather_data_{identifier}")
             .Add(Constants.Predicates.Type, Constants.Types.WeatherData)
                .Add(Constants.Predicates.WeatherDataStation, measurement.station)
                .Add(Constants.Predicates.WeatherDataAlti, measurement.alti)
                .Add(Constants.Predicates.WeatherDataDrct, measurement.drct)
                .Add(Constants.Predicates.WeatherDataDwpc, measurement.dwpc)
                .Add(Constants.Predicates.WeatherDataDwpf, measurement.dwpf)
                .Add(Constants.Predicates.WeatherDataFeelc, measurement.feelc)
                .Add(Constants.Predicates.WeatherDataFeelf, measurement.feelf)
                .Add(Constants.Predicates.WeatherDataGust, measurement.gust)
                .Add(Constants.Predicates.WeatherDataIceAccretion1hr, measurement.ice_accretion_1hr)
                .Add(Constants.Predicates.WeatherDataIceAccretion3hr, measurement.ice_accretion_3hr)
                .Add(Constants.Predicates.WeatherDataIceAccretion6hr, measurement.ice_accretion_6hr)
                .Add(Constants.Predicates.WeatherDataLatitude, measurement.lat)
                .Add(Constants.Predicates.WeatherDataLongitude, measurement.lon)
                .Add(Constants.Predicates.WeatherDataMetar, measurement.metar)
                .Add(Constants.Predicates.WeatherDataMslp, measurement.mslp)
                .Add(Constants.Predicates.WeatherDataP01i, measurement.p01i)
                .Add(Constants.Predicates.WeatherDataPeakWindDrct, measurement.peak_wind_drct)
                .Add(Constants.Predicates.WeatherDataPeakWindGust, measurement.peak_wind_gust)
                .Add(Constants.Predicates.WeatherDataPeakWindTime_hh, measurement.peak_wind_time_hh)
                .Add(Constants.Predicates.WeatherDataPeakWindTime_MM, measurement.peak_wind_time_MM)
                .Add(Constants.Predicates.WeatherDataRelh, measurement.relh)
                .Add(Constants.Predicates.WeatherDataSknt, measurement.sknt)
                .Add(Constants.Predicates.WeatherDataSkyc1, measurement.skyc1)
                .Add(Constants.Predicates.WeatherDataSkyc2, measurement.skyc2)
                .Add(Constants.Predicates.WeatherDataSkyc3, measurement.skyc3)
                .Add(Constants.Predicates.WeatherDataSkyc4, measurement.skyc4)
                .Add(Constants.Predicates.WeatherDataSkyl1, measurement.skyl1)
                .Add(Constants.Predicates.WeatherDataSkyl2, measurement.skyl2)
                .Add(Constants.Predicates.WeatherDataSkyl3, measurement.skyl3)
                .Add(Constants.Predicates.WeatherDataSkyl4, measurement.skyl4)
                .Add(Constants.Predicates.WeatherDataTimestamp, measurement.valid)
                .Add(Constants.Predicates.WeatherDataTmpc, measurement.tmpc)
                .Add(Constants.Predicates.WeatherDataTmpf, measurement.tmpf)
                .Add(Constants.Predicates.WeatherDataVsbyKm, measurement.vsby_km)
                .Add(Constants.Predicates.WeatherDataVsbyMi, measurement.vsby_mi)
                .Add(Constants.Predicates.WeatherDataWxcodes, measurement.wxcodes);

            // TODO Here we need a Lookup for the Station ID, because we can't be sure there will be blank node for it:
            builder.AddEdge(Constants.Predicates.HasWeatherStation, $"_:weather_station_{measurement.station}");

            return builder.Build();
        }


        private static List<NQuad> ConvertToNquad(string identifier, FlightDto flight)
        {
            // We use flight_{identifier} as the Subjects ID. That's because 
            // we don't want to put too much logic in here to build a very 
            // unique subject id:
            var builder = new NQuadBuilder($"_:flight_{identifier}")
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
                .Add(Constants.Predicates.FlightWeatherDelay, flight.WeatherDelay);

            // Add CancellationCode:
            if (!string.IsNullOrWhiteSpace(flight.CancellationCode))
            {
                builder.Add(Constants.Predicates.FlightCancellationCode, flight.CancellationCode);
            }

            // Set Airports:
            builder.AddEdge(Constants.Predicates.HasOriginAirport, $"_:airport_{flight.OriginAirport}");
            builder.AddEdge(Constants.Predicates.HasDestinationAirport, $"_:airport_{flight.DestinationAirport}");

            // Set Carrier:
            builder.AddEdge(Constants.Predicates.HasCarrier, $"_:carrier_{flight.Carrier}");


            return builder.Build();
        }
    }
}
