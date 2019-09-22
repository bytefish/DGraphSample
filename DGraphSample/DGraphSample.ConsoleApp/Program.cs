// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DGraphSample.Csv.Aotp.Parser;
using DGraphSample.DGraph.Dto;
using DGraphSample.DGraph.Processors;
using DGraphSample.DGraph.Queries;
using DGraphSample.DGraph.Resolvers;
using Grpc.Core;
using TinyCsvParser;
using TinyDgraphClient.Client;
using TinyDgraphClient.Generated;

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
            Main().GetAwaiter().GetResult();
        }

        public static async Task Main()
        {
            var client = new DGraphClient("127.0.0.1", 9080, ChannelCredentials.Insecure);

            // Drop All:
            await client.AlterAsync(new Operation { DropAll = true }, CancellationToken.None);

            // Create the Schema and Drop all data for this test:
            await client.AlterAsync(new Operation { Schema = Query.Schema }, CancellationToken.None);
            
            // Insert Data:
            await InsertAirports(client);
            await InsertCarrierData(client);
            await InsertFlightData(client);
        }

        private static async Task InsertAirports(DGraphClient client)
        {
            var airports = GetAirportData(csvAirportFile).ToList();
            var processor = new AirportBatchProcessor(client);

            await processor.ProcessAsync(airports, CancellationToken.None);
        }

        private static async Task InsertCarrierData(DGraphClient client)
        {
            var carriers = GetCarrierData(csvCarriersFile).ToList();
            var processor = new CarrierBatchProcessor(client);

            await processor.ProcessAsync(carriers, CancellationToken.None);
        }

        private static async Task InsertFlightData(DGraphClient client)
        {
            // We cache the data, that fits into memory. Airports and Carriers won't 
            // change during the initial data ingestion, so we can cache it in memory 
            // to prevent useless network calls:
            var airportResolver = await AirportResolver.CreateResolverAsync(client);
            var carrierResolver = await CarrierResolver.CreateResolverAsync(client);

            // Create the Processor and use the Resolvers:
            var processor = new FlightBatchProcessor(client, airportResolver, carrierResolver);

            int totalFlightNum = 0;

            // Create Flight Data with Batched Items:
            foreach (var csvFlightStatisticsFile in csvFlightStatisticsFiles)
            {
                Console.WriteLine($@"Starting Flights CSV Import: {csvFlightStatisticsFile}");

                GetFlightData(csvFlightStatisticsFile)
                    // As an Observable:
                    .ToObservable()
                    // Batch in Entities / Wait:
                    .Buffer(TimeSpan.FromSeconds(1), 100)
                    // Insert when Buffered:
                    .Subscribe(records =>
                    {
                        if (records.Count > 0)
                        {
                            processor.ProcessAsync(records, CancellationToken.None).GetAwaiter().GetResult();

                            totalFlightNum += records.Count;

                            Console.WriteLine($"[{DateTime.Now}] Wrote {totalFlightNum} Flights ...");
                        }
                    });
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
                    FlightDate =  x.FlightDate
                });
        }

        #endregion
    }
}