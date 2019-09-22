// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using DGraphSample.Csv.Aotp.Parser;
using DGraphSample.Dto;
using DGraphSample.Export;
using DGraphSample.Resolvers;
using TinyCsvParser;

namespace DGraphSample.ConsoleApp
{
    public class Program
    {
        private static string csvAirportFile = @"D:\flights_data\aotp\master_cord.csv";
        private static string csvCarriersFile = @"D:\flights_data\aotp\unqiue_carriers.csv";
        private static string csvWeatherStationsFileName = @"D:\flights_data\ncar\stations.txt";

        private static string[] csvFlightStatisticsFiles
        {
            get
            {
                return Directory.GetFiles(@"D:\flights_data\aotp\2014", "*.csv");
            }
        }

        private static string[] csvWeatherDataFiles
        {
            get
            {
                return Directory.GetFiles(@"D:\flights_data\asos\2014", "*.txt");
            }
        }

        public static void Main(string[] args)
        {
            const string rdfFilePath = "weather_flights_dataset.rdf";

            StreamWriter writer = new StreamWriter(rdfFilePath, true);

            //WriteAirports(writer);
            //WriteCarriers(writer);
            //WriteFlights(writer);
            //WriteWeatherStations(writer);
            WriteWeatherData(writer);

            writer.Flush();
            writer.Close();
        }

        private static void WriteAirports(StreamWriter writer)
        {
            var airports = GetAirportData(csvAirportFile).AsEnumerable();

            foreach (var airport in airports)
            {
                var lines = FlightsRdfExporter.ConvertToRdf(airport.AirportId, airport);

                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void WriteCarriers(StreamWriter writer)
        {
            var carriers = GetCarrierData(csvCarriersFile).AsEnumerable();

            foreach (var carrier in carriers)
            {
                var lines = FlightsRdfExporter.ConvertToRdf(carrier.Code, carrier);

                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void WriteFlights(StreamWriter writer)
        {
            ulong pos = 0;

            foreach (var csvFlightStatisticsFile in csvFlightStatisticsFiles)
            {
                var flights = GetFlightData(csvFlightStatisticsFile).AsEnumerable();

                foreach (var flight in flights)
                {
                    var lines = FlightsRdfExporter.ConvertToRdf($"{pos}", flight);

                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    pos++;
                }
            }
        }

        private static void WriteWeatherStations(StreamWriter writer)
        {
            var stations = GetWeatherStationData(csvWeatherStationsFileName).AsEnumerable();

            foreach (var station in stations)
            {
                var lines = FlightsRdfExporter.ConvertToRdf(station.IATA, station);

                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void WriteWeatherData(StreamWriter writer)
        {
            ulong pos = 0;

            foreach (var csvWeatherDataFile in csvWeatherDataFiles)
            {
                var measurements = GetWeatherData(csvWeatherDataFile).AsEnumerable();

                foreach (var measurement in measurements)
                {
                    var lines = FlightsRdfExporter.ConvertToRdf($"{pos}", measurement);

                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }
                }

                pos++;
            }
        }

        #region CSV Parsing 

        private static ParallelQuery<CarrierDto> GetCarrierData(string filename)
        {
            return Parsers.CarrierParser
                // Parse as ASCII file:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only use valid lines:
                .Where(x => x.IsValid)
                // Get the Result:
                .Select(x => x.Result)
                // Get Carrier:
                .Select(x => new CarrierDto
                {
                    Code = x.Code,
                    Description = x.Description
                });
        }

        private static ParallelQuery<AirportDto> GetAirportData(string filename)
        {
            return Parsers.AirportParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Only select the latest available data:
                .Where(x => x.AirportIsLatest)
                // Build the intermediate Airport Information:
                .Select(x => new AirportDto
                {
                    AirportId = x.AirportId,
                    IATA = x.AirportIata,
                    Name = x.AirportName,
                    City = x.AirportCityName,
                    Country = x.AirportCountryName,
                    State = x.AirportStateName,
                });
        }

        private static ParallelQuery<FlightDto> GetFlightData(string filename)
        {
            return Parsers.FlightStatisticsParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Return the Graph Model:
                .Select(x => new FlightDto
                {
                    FlightNumber = x.FlightNumber,
                    Year = x.Year,
                    Month = x.Month,
                    DayOfMonth = x.DayOfMonth,
                    DayOfWeek = x.DayOfWeek,
                    Carrier = x.UniqueCarrier,
                    OriginAirport = x.OriginAirport,
                    DestinationAirport = x.DestinationAirport,
                    TailNumber = x.TailNumber,
                    ArrivalDelay = x.ArrivalDelay,
                    CancellationCode = x.CancellationCode,
                    DepartureDelay = x.DepartureDelay,
                    CarrierDelay = x.CarrierDelay,
                    Distance = x.Distance,
                    LateAircraftDelay = x.LateAircraftDelay,
                    NasDelay = x.NasDelay,
                    SecurityDelay = x.SecurityDelay,
                    WeatherDelay = x.WeatherDelay,
                    FlightDate = x.FlightDate
                });
        }

        private static ParallelQuery<WeatherStationDto> GetWeatherStationData(string filename)
        {
            // Build a Lookup Table to get the Airport by IATA:
            var availableAirportsInData = GetAirportData(csvAirportFile)
                .GroupBy(x => x.IATA).Select(x => x.First()) // Apparently we have duplicates in the data ...
                .ToDictionary(x => x.IATA, x => x);

            return Csv.Ncar.Parser.Parsers.MetarStationParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Only use Stations with available Airports:
                .Where(x => availableAirportsInData.ContainsKey(x.IATA))
                // Return the Graph Model:
                .Select(x => new WeatherStationDto
                {
                    Station = x.Station,
                    Elevation = x.Elevation,
                    IATA = x.IATA,
                    ICAO = x.ICAO,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    SYNOP = x.SYNOP
                });
        }

        private static ParallelQuery<WeatherDataDto> GetWeatherData(string filename)
        {
            var availableStations = GetWeatherStationData(csvWeatherStationsFileName)
                .GroupBy(x => x.ICAO).Select(x => x.First()) // Apparently we have duplicates in the data ...
                .ToDictionary(x => x.ICAO, x => x);

            return Csv.Asos.Parser.Parsers.AsosDatasetParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Only use measurement if the station is available:
                .Where(x => availableStations.ContainsKey(x.station))
                // Return the Graph Model:
                .Select(x => new WeatherDataDto
                {
                    alti = x.alti,
                    drct = x.drct,
                    dwpf = x.dwpf,
                    dwpc = ConvertFahrenheitToCelsius(x.dwpf),
                    feelf = x.feel,
                    feelc = ConvertFahrenheitToCelsius(x.feel),
                    gust = x.gust,
                    ice_accretion_1hr = x.ice_accretion_1hr,
                    ice_accretion_3hr = x.ice_accretion_3hr,
                    ice_accretion_6hr = x.ice_accretion_6hr,
                    lat = x.lat,
                    lon = x.lon,
                    metar = x.metar,
                    mslp = x.mslp,
                    p01i = x.p01i,
                    peak_wind_drct = x.peak_wind_drct,
                    peak_wind_gust = x.peak_wind_gust,
                    peak_wind_time_hh = x.peak_wind_time.HasValue ? x.peak_wind_time.Value.Hours : default(int?),
                    peak_wind_time_MM = x.peak_wind_time.HasValue ? x.peak_wind_time.Value.Minutes : default(int?),
                    relh = x.relh,
                    sknt = x.sknt,
                    skyc1 = x.skyc1,
                    skyc2 = x.skyc2,
                    skyc3 = x.skyc3,
                    skyc4 = x.skyc4,
                    skyl1 = x.skyl1,
                    skyl2 = x.skyl2,
                    skyl3 = x.skyl3,
                    skyl4 = x.skyl4,
                    station = x.station,
                    tmpf = x.tmpf,
                    tmpc = ConvertFahrenheitToCelsius(x.tmpf),
                    valid = x.valid,
                    vsby_mi = x.vsby,
                    vsby_km = ConvertMilesToKilometers(x.vsby),
                    wxcodes = x.wxcodes,
                });            
        }

        #endregion

        #region Unit Converters

        public static float? ConvertFahrenheitToCelsius(float? fahrenheit)
        {
            if (!fahrenheit.HasValue)
            {
                return default(float?);
            }

            return (fahrenheit.Value - 32.0f) * 5.0f / 9.0f;
        }

        public static float ConvertFahrenheitToCelsius(float fahrenheit)
        {
            return (fahrenheit - 32.0f) * 5.0f / 9.0f;
        }

        public static float? ConvertMilesToKilometers(float? miles)
        {
            if (!miles.HasValue)
            {
                return default(float?);
            }

            return miles.Value * 1.609344f;
        }

        public static float ConvertMilesToKilometers(float miles)
        {
            return miles * 1.609344f;
        }

        #endregion
    }
}