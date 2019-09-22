// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Text;
using DGraphSample.Csv.Aotp.Parser;
using DGraphSample.DGraph.Dto;
using DGraphSample.Export;
using TinyCsvParser;

namespace DGraphSample.ConsoleApp
{
    public class Program
    {
        private static readonly string csvAirportFile = @"D:\github\LearningNeo4jAtScale\Resources\56803256_T_MASTER_CORD.csv";

        private static readonly string csvCarriersFile = @"D:\github\LearningNeo4jAtScale\Resources\UNIQUE_CARRIERS.csv";

        private static readonly string[] csvFlightStatisticsFiles = new[]
        {
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201401.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201402.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201403.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201404.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201405.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201406.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201407.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201408.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201409.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201410.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201411.csv",
            "D:\\datasets\\AOTP\\ZIP\\AirOnTimeCSV_1987_2017\\AirOnTimeCSV\\airOT201412.csv",
        };

        public static void Main(string[] args)
        {
            const string rdfFilePath = "flights.rdf";

            StreamWriter writer = new StreamWriter(rdfFilePath, true);

            WriteAirports(writer);
            WriteCarriers(writer);
            WriteFlights(writer);

            writer.Flush();
            writer.Close();
        }

        private static void WriteAirports(StreamWriter writer)
        {
            var airports = GetAirportData(csvAirportFile).AsEnumerable();

            foreach (var airport in airports)
            {
                var lines = FlightsRdfExporter.ConvertToRdf(airport.Iata, airport);

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
            ulong totalNumberOfFlightsWritten = 0;

            // Create Flight Data with Batched Items:
            foreach (var csvFlightStatisticsFile in csvFlightStatisticsFiles)
            {
                Console.WriteLine($@"Starting Flights CSV Import: {csvFlightStatisticsFile}");

                var flights = GetFlightData(csvFlightStatisticsFile).AsEnumerable();

                foreach (var flight in flights)
                {
                    var lines = FlightsRdfExporter.ConvertToRdf($"{totalNumberOfFlightsWritten}", flight);

                    foreach (var line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    totalNumberOfFlightsWritten = totalNumberOfFlightsWritten + 1;
                }
            }
        }

        #region CSV Parsing 

        private static ParallelQuery<DGraph.Model.CarrierDto> GetCarrierData(string filename)
        {
            return Parsers.CarrierParser
                // Parse as ASCII file:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only use valid lines:
                .Where(x => x.IsValid)
                // Get the Result:
                .Select(x => x.Result)
                // Get Carrier:
                .Select(x => new DGraph.Model.CarrierDto
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
                    Iata = x.AirportId,
                    Name = x.AirportName,
                    Abbr = x.AirportAbbr,
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

        #endregion
    }
}