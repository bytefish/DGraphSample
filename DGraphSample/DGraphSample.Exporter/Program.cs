// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using DGraphSample.Csv.Aotp.Parser;
using DGraphSample.Exporter.Dto;
using Microsoft.Extensions.Logging;
using TinyCsvParser;
using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Parsing;
using VDS.RDF.Writing;
using VDS.RDF.Writing.Formatting;

namespace DGraphSample.Exporter
{
    public class CustomNTriplesFormatter : BaseFormatter
    {
        /// <summary>
        /// Set of characters which must be escaped in Literals.
        /// </summary>
        private readonly List<string[]> _litEscapes = new List<string[]>
        {
            new [] { @"\", @"\\" },
            new [] { "\"", "\\\"" },
            new [] { "\n", @"\n" },
            new [] { "\r", @"\r" },
            new [] { "\t", @"\t" },
        };

        /// <summary>
        /// Creates a new NTriples formatter.
        /// </summary>
        /// <param name="syntax">NTriples syntax to output.</param>
        /// <param name="formatName">Format Name.</param>
        public CustomNTriplesFormatter(NTriplesSyntax syntax, string formatName)
            : base(formatName)
        {
            Syntax = syntax;
        }

        /// <summary>
        /// Creates a new NTriples Formatter.
        /// </summary>
        public CustomNTriplesFormatter(NTriplesSyntax syntax)
            : this(syntax, GetName(syntax)) { }

        /// <summary>
        /// Creates a new NTriples Formatter.
        /// </summary>
        public CustomNTriplesFormatter()
            : this(NTriplesSyntax.Original, GetName()) { }

        /// <summary>
        /// Creates a new NTriples Formatter.
        /// </summary>
        /// <param name="formatName">Format Name.</param>
        protected CustomNTriplesFormatter(string formatName)
            : this(NTriplesSyntax.Original, formatName) { }

        private static string GetName(NTriplesSyntax syntax = NTriplesSyntax.Original)
        {
            switch (syntax)
            {
                case NTriplesSyntax.Original:
                    return "NTriples";
                default:
                    return "NTriples (RDF 1.1)";
            }
        }

        /// <summary>
        /// Gets the NTriples syntax being used.
        /// </summary>
        public NTriplesSyntax Syntax { get; private set; }

        /// <summary>
        /// Formats a URI Node.
        /// </summary>
        /// <param name="u">URI Node.</param>
        /// <param name="segment">Triple Segment.</param>
        /// <returns></returns>
        protected override string FormatUriNode(IUriNode u, TripleSegment? segment)
        {
            var output = new StringBuilder();
            output.Append('<');
            output.Append(FormatUri(u.Uri));
            output.Append('>');
            return output.ToString();
        }


        /// <summary>
        /// Formats a Literal Node.
        /// </summary>
        /// <param name="l">Literal Node.</param>
        /// <param name="segment">Triple Segment.</param>
        /// <returns></returns>
        protected override string FormatLiteralNode(ILiteralNode l, TripleSegment? segment)
        {
            var output = new StringBuilder();

            output.Append('"');
            var value = l.Value;
            value = Escape(value, _litEscapes);
            output.Append(FormatChar(value.ToCharArray()));
            output.Append('"');

            if (!l.Language.Equals(string.Empty))
            {
                output.Append('@');
                output.Append(l.Language.ToLower());
            }
            else if (l.DataType != null)
            {
                output.Append("^^<");
                output.Append(FormatUri(l.DataType));
                output.Append('>');
            }

            return output.ToString();
        }

        [Obsolete("This form of the FormatChar() method is considered obsolete as it is inefficient", false)]
        public override string FormatChar(char c)
        {
            if (Syntax != NTriplesSyntax.Original) return base.FormatChar(c);
            if (c <= 127)
            {
                // ASCII
                return c.ToString();
            }
            // Small Unicode Escape required
            return "\\u" + ((int)c).ToString("X4");
        }

        public override string FormatChar(char[] cs)
        {
            if (Syntax != NTriplesSyntax.Original) return base.FormatChar(cs);

            StringBuilder builder = new StringBuilder();
            int start = 0, length = 0;
            for (int i = 0; i < cs.Length; i++)
            {
                char c = cs[i];
                if (c <= 127)
                {
                    length++;
                }
                else
                {
                    builder.Append(cs, start, length);
                    start = i + 1;
                    length = 0;
                    builder.Append("\\u");
                    builder.Append(((int)c).ToString("X4"));
                }
            }
            if (length == cs.Length)
            {
                return new string(cs);
            }
            if (length > 0) builder.Append(cs, start, length);
            return builder.ToString();
        }

        protected override string FormatBlankNode(IBlankNode b, TripleSegment? segment)
        {
            return "_:" + b.InternalID;
        }

        public override string FormatUri(Uri u)
        {
            if(u == null)
            {
                return string.Empty;
            }
            
            // Honestly this is a bit brittle. We just want to use the AbsoluteUri,
            // if the Uri starts with an XMLSchema definition:
            if(u.ToString().StartsWith("http://www.w3.org/2001/XMLSchema"))
            {
                return u.AbsoluteUri;
            }

            // In dotnetrdf the Formatters expexts a valid Uri for a Uri Node. We cannot 
            // use "<airport.airport_id>" for a predicate. At the same time I don't want 
            // to have "<http://www.bytefish.de/Aviation/Airports#airport.airport_id> for 
            // predicates. They would make queries extremly painful.
            //
            // We use a little knowledge about the predicates used in this code. We only 
            // use the Fragment of the Uri as the predicate. Maybe this doesn't hold for 
            // the RDF you need to write, so please be careful using this code:
            return u
                .Fragment
                .TrimStart('#');
        }

        public override string FormatUri(string u)
        {
            var uri = new Uri(u);

            return FormatUri(uri);
        }
    }

    public static class DotNetRdfHelpers
    {
        public static INode AsVariableNode(this INodeFactory nodeFactory, string value)
        {
            return nodeFactory.CreateVariableNode(value);
        }

        public static INode AsUriNode(this INodeFactory nodeFactory, Uri uri)
        {
            return nodeFactory.CreateUriNode(uri);
        }

        public static INode AsLiteralNode(this INodeFactory nodeFactory, string value)
        {
            return nodeFactory.CreateLiteralNode(value);
        }

        public static INode AsLiteralNode(this INodeFactory nodeFactory, string value, string langspec)
        {
            return nodeFactory.CreateLiteralNode(value, langspec);
        }

        public static INode AsBlankNode(this INodeFactory nodeFactory, string nodeId)
        {
            return nodeFactory.CreateBlankNode(nodeId);
        }

        public static INode AsValueNode(this INodeFactory nodeFactory, object value)
        {
            if (value == null)
            {
                return null;
            }

            switch (value)
            {
                case Uri uriValue:
                    return nodeFactory.CreateUriNode(uriValue);

                case bool boolValue:
                    return new BooleanNode(null, boolValue);

                case byte byteValue:
                    return new ByteNode(null, byteValue);

                case DateTime dateTimeValue:
                    return new DateTimeNode(null, dateTimeValue);

                case DateTimeOffset dateTimeOffsetValue:
                    return new DateTimeNode(null, dateTimeOffsetValue);

                case decimal decimalValue:
                    return new DecimalNode(null, decimalValue);

                case double doubleValue:
                    return new DoubleNode(null, doubleValue);

                case float floatValue:
                    return new FloatNode(null, floatValue);

                case long longValue:
                    return new LongNode(null, longValue);

                case int intValue:
                    return new LongNode(null, intValue);

                case string stringValue:
                    return new StringNode(null, stringValue, UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeString));

                case char charValue:
                    return new StringNode(null, charValue.ToString(), UriFactory.Create(XmlSpecsHelper.XmlSchemaDataTypeString));

                case TimeSpan timeSpanValue:
                    return new TimeSpanNode(null, timeSpanValue);

                default:
                    throw new InvalidOperationException($"Can't convert type {value.GetType()}");
            }
        }
    }

    public class NonCachingQNameOutputMapper : QNameOutputMapper
    {
        public NonCachingQNameOutputMapper(INamespaceMapper nsmapper)
            : base(nsmapper)
        {
        }

        protected override void AddToCache(string uri, QNameMapping mapping)
        {
            // Ignore ...
        }
    }

    /// <summary>
    /// A simple Generator for Blank Node IDs. I don't need any validation / caching or complicated mechanisms
    /// for generating Blank Node IDs. We will instantiate this Generator and generate unique IDs among 
    /// all entities to be used.
    /// </summary>
    public class NodeIdGenerator
    {
        private long nodeId;

        public NodeIdGenerator()
        {
            nodeId = 0;
        }

        public string GetNextId()
        {
            return Interlocked.Increment(ref nodeId).ToString();
        }
    }

    public class Program
    {
        /// <summary>
        /// Master Coordination Data.
        /// </summary>
        private static string csvAirportFile = @"D:\flights_data\aotp\master_cord.csv";

        /// <summary>
        /// FAA Aircraft Data.
        /// </summary>
        private static string csvAircraftsFile = @"D:\flights_data\faa\FAA_AircraftRegistration_Database.csv";

        /// <summary>
        /// Carriers.
        /// </summary>
        private static string csvCarriersFile = @"D:\flights_data\aotp\unqiue_carriers.csv";

        /// <summary>
        /// Weather Stations.
        /// </summary>
        private static string csvWeatherStationsFileName = @"D:\flights_data\ncar\stations.txt";

        /// <summary>
        /// Flight Data.
        /// </summary>
        private static string[] csvFlightStatisticsFiles
        {
            get
            {
                return Directory.GetFiles(@"D:\flights_data\aotp\2014", "*.csv");
            }
        }

        /// <summary>
        /// Weather Data.
        /// </summary>
        private static string[] csvWeatherDataFiles
        {
            get
            {
                return Directory.GetFiles(@"D:\flights_data\asos\2014", "*.txt");
            }
        }
        
        private static INodeFactory nodeFactory;

        private static CustomNTriplesFormatter nTriplesFormatter;

        private static NamespaceMapper namespaceMapper;

        private static ILogger log;

        private static NodeIdGenerator nodeIdGenerator;

        public static void Main(string[] args)
        {
            // Simple NodeId Generator for BlankNode usage:
            nodeIdGenerator = new NodeIdGenerator();

            // Create a Console Logger:
            log = LoggerFactory
                .Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .CreateLogger<Program>();

            // We don't want to use a Graph to prevent Memory 
            // from exploding while writing the Data:
            nodeFactory = new NodeFactory();

            // We don't want to write the full URL for every 
            // triple, because that will lead to a large TTL 
            // file with redundant data:
            namespaceMapper = new NamespaceMapper();

            namespaceMapper.AddNamespace("ge", Constants.NsAviationGeneral);
            namespaceMapper.AddNamespace("ap", Constants.NsAviationAirport);
            namespaceMapper.AddNamespace("ac", Constants.NsAviationtAircraft);
            namespaceMapper.AddNamespace("ca", Constants.NsAviationCarrier);
            namespaceMapper.AddNamespace("fl", Constants.NsAviationFlight);
            namespaceMapper.AddNamespace("we", Constants.NsAviationWeather);
            namespaceMapper.AddNamespace("st", Constants.NsAviationWeatherStation);

            // Create the TurtleFormatter with the Namespace Mappings:
            nTriplesFormatter = new CustomNTriplesFormatter();

            // Load the Base Data:
            log.LogDebug("Loading Base Data ...");

            var aircrafts = GetAircraftData(csvAircraftsFile).ToList();
            var carriers = GetCarrierData(csvCarriersFile).ToList();
            var airports = GetAirportData(csvAirportFile).ToList();
            var stations = GetWeatherStationData(csvWeatherStationsFileName).ToList();

            using (FileStream fileStream = File.Create(@"G:\aviation_2014.ttl"))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    WriteAircrafts(writer, aircrafts);
                    WriteAirports(writer, airports);
                    WriteCarriers(writer, carriers);
                    WriteFlights(writer, aircrafts, airports, carriers);
                    WriteWeatherStations(writer, stations, airports);
                    WriteWeatherDatas(writer, stations);
                }
            }
        }

        private static void WriteAircrafts(StreamWriter streamWriter, IEnumerable<AircraftDto> aircrafts)
        {
            log.LogDebug($"Writing Aircrafts: {csvAircraftsFile} ...");

            foreach (var triple in aircrafts
                .SelectMany(x => ConvertAircraft(x)))
            {
                WriteTriple(streamWriter, triple);
            }
        }

        private static List<Triple> ConvertAircraft(AircraftDto aircraft)
        {
            return new TripleBuilder(nodeFactory.AsBlankNode(aircraft.NodeId))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.Aircraft))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftEngineHorsepower), nodeFactory.AsValueNode(aircraft.EngineHorsepower))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftEngineManufacturer), nodeFactory.AsValueNode(aircraft.EngineManufacturer))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftEngineModel), nodeFactory.AsValueNode(aircraft.EngineModel))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftEngineThrust), nodeFactory.AsValueNode(aircraft.EngineThrust))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftManufacturer), nodeFactory.AsValueNode(aircraft.AircraftManufacturer))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftModel), nodeFactory.AsValueNode(aircraft.AircraftModel))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftN_Number), nodeFactory.AsValueNode(aircraft.N_Number))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftSeats), nodeFactory.AsValueNode(aircraft.AircraftSeats))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftSerialNumber), nodeFactory.AsValueNode(aircraft.SerialNumber))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AircraftUniqueId), nodeFactory.AsValueNode(aircraft.UniqueId))
                    .Build();
        }

        private static void WriteAirports(StreamWriter streamWriter, IEnumerable<AirportDto> airports)
        {
            log.LogDebug($"Writing Airports: {csvAirportFile} ...");

            foreach (var triple in airports
                .SelectMany(x => ConvertAirport(x)))
            {
                WriteTriple(streamWriter, triple);
            }
        }

        private static List<Triple> ConvertAirport(AirportDto airport)
        {
            return new TripleBuilder(nodeFactory.AsBlankNode(airport.NodeId))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.Airport))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportId), nodeFactory.AsValueNode(airport.AirportId))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportIata), nodeFactory.AsValueNode(airport.IATA))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportName), nodeFactory.AsValueNode(airport.Name))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportCity), nodeFactory.AsValueNode(airport.City))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportState), nodeFactory.AsValueNode(airport.State))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.AirportCountry), nodeFactory.AsValueNode(airport.Country))
                    .Build();
        }

        private static void WriteCarriers(StreamWriter streamWriter, IEnumerable<CarrierDto> carriers)
        {
            log.LogDebug($"Writing Carriers: {csvCarriersFile} ...");

            foreach (var triple in carriers
                .SelectMany(x => ConvertCarrier(x)))
            {
                WriteTriple(streamWriter, triple);
            }
        }

        private static List<Triple> ConvertCarrier(CarrierDto carrier)
        {
            return new TripleBuilder(nodeFactory.AsBlankNode(carrier.NodeId))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.Carrier))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.CarrierCode), nodeFactory.AsValueNode(carrier.Code))
                    .Add(nodeFactory.AsUriNode(Constants.Predicates.CarrierDescription), nodeFactory.AsValueNode(carrier.Description))
                    .Build();
        }

        private static void WriteFlights(StreamWriter streamWriter, List<AircraftDto> aircrafts, List<AirportDto> airports, List<CarrierDto> carriers)
        {
            // Build Lookup Tables. We group by a criteria, to prevent duplicates 
            // from being used as dictionary keys.
            var aircraftNodes = aircrafts
                .GroupBy(x => x.N_Number).Select(x => x.First())
                .ToDictionary(x => x.N_Number, x => x);

            var airportNodes = airports
                .GroupBy(x => x.AirportId).Select(x => x.First())
                .ToDictionary(x => x.AirportId, x => x);

            var carrierNodes = carriers
                .GroupBy(x => x.Code).Select(x => x.First())
                .ToDictionary(x => x.Code, x => x);

            foreach (var csvFlightStatisticsFile in csvFlightStatisticsFiles)
            {
                log.LogDebug($"Writing Flights: {csvFlightStatisticsFile} ...");

                var flights = GetFlightData(csvFlightStatisticsFile);

                foreach (var triple in flights
                    .SelectMany(x => ConvertFlight(x, aircraftNodes, airportNodes, carrierNodes)))
                {
                    WriteTriple(streamWriter, triple);
                }
            }
        }

        private static List<Triple> ConvertFlight(FlightDto flight, Dictionary<string, AircraftDto> aircrafts, Dictionary<string, AirportDto> airports, Dictionary<string, CarrierDto> carriers)
        {
            var triples = new TripleBuilder(nodeFactory.AsBlankNode(flight.NodeId));

            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.Flight));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightNumber), nodeFactory.AsValueNode(flight.FlightNumber));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightTailNumber), nodeFactory.AsValueNode(flight.TailNumber));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightDate), nodeFactory.AsValueNode(flight.FlightDate));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightDayOfWeek), nodeFactory.AsValueNode(flight.DayOfWeek));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightDayOfMonth), nodeFactory.AsValueNode(flight.DayOfMonth));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightMonth), nodeFactory.AsValueNode(flight.Month));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightYear), nodeFactory.AsValueNode(flight.Year));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightDistance), nodeFactory.AsValueNode(flight.Distance));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightArrivalDelay), nodeFactory.AsValueNode(flight.ArrivalDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightCarrierDelay), nodeFactory.AsValueNode(flight.CarrierDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightDepartureDelay), nodeFactory.AsValueNode(flight.DepartureDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightLateAircraftDelay), nodeFactory.AsValueNode(flight.LateAircraftDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightNasDelay), nodeFactory.AsValueNode(flight.NasDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightSecurityDelay), nodeFactory.AsValueNode(flight.SecurityDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightCancellationCode), nodeFactory.AsValueNode(flight.CancellationCode));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightWeatherDelay), nodeFactory.AsValueNode(flight.WeatherDelay));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightScheduledDepartureTime), nodeFactory.AsValueNode(flight.ScheduledDepartureTime));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.FlightActualDepartureTime), nodeFactory.AsValueNode(flight.ActualDepartureTime));

            // Add Relations:
            if (aircrafts.TryGetValue(flight.TailNumber, out AircraftDto aircraft))
            {
                triples.Add(nodeFactory.AsUriNode(Constants.Predicates.HasAircraft), nodeFactory.AsBlankNode(aircraft.NodeId));
            }

            if (airports.TryGetValue(flight.OriginAirport, out AirportDto originAirport))
            {
                triples.Add(nodeFactory.AsUriNode(Constants.Predicates.HasOriginAirport), nodeFactory.AsBlankNode(originAirport.NodeId));
            }

            if (airports.TryGetValue(flight.DestinationAirport, out AirportDto destinationAirport))
            {
                triples.Add(nodeFactory.AsUriNode(Constants.Predicates.HasDestinationAirport), nodeFactory.AsBlankNode(destinationAirport.NodeId));
            }

            if (carriers.TryGetValue(flight.Carrier, out CarrierDto carrier))
            {
                triples.Add(nodeFactory.AsUriNode(Constants.Predicates.HasCarrier), nodeFactory.AsBlankNode(carrier.NodeId));
            }

            return triples.Build();
        }

        private static void WriteWeatherStations(StreamWriter streamWriter, IEnumerable<WeatherStationDto> stations, IEnumerable<AirportDto> airports)
        {
            log.LogDebug($"Writing Weather Stations: {csvWeatherStationsFileName} ...");

            var airportNodes = airports
                .GroupBy(x => x.IATA).Select(x => x.First())
                .ToDictionary(x => x.IATA, x => x);

            foreach (var triple in stations
                .SelectMany(x => ConvertWeatherStation(x, airportNodes)))
            {
                WriteTriple(streamWriter, triple);
            }
        }

        private static List<Triple> ConvertWeatherStation(WeatherStationDto station, Dictionary<string, AirportDto> airports)
        {
            var triples = new TripleBuilder(nodeFactory.AsBlankNode(station.NodeId));

            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.WeatherStation));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationIata), nodeFactory.AsValueNode(station.IATA));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationName), nodeFactory.AsValueNode(station.Station));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationIcao), nodeFactory.AsValueNode(station.ICAO));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationLat), nodeFactory.AsValueNode(station.Latitude));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationLon), nodeFactory.AsValueNode(station.Longitude));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationSynop), nodeFactory.AsValueNode(station.SYNOP));
            triples.Add(nodeFactory.AsUriNode(Constants.Predicates.WeatherStationElevation), nodeFactory.AsValueNode(station.Elevation));


            if (airports.TryGetValue(station.IATA, out AirportDto airport))
            {
                triples.AddWithSubject(
                    subj: nodeFactory.AsBlankNode(airport.NodeId),
                    pred: nodeFactory.AsUriNode(Constants.Predicates.HasWeatherStation),
                    obj: nodeFactory.AsBlankNode(station.NodeId));
            }

            return triples.Build();
        }

        private static void WriteWeatherDatas(StreamWriter streamWriter, IEnumerable<WeatherStationDto> stations)
        {
            var stationNodes = stations
                .GroupBy(x => x.IATA).Select(x => x.First())
                .ToDictionary(x => x.IATA, x => x);

            foreach (var csvWeatherDataFile in csvWeatherDataFiles)
            {
                log.LogDebug($"Writing Weather Data: {csvWeatherDataFile} ...");

                var weatherDataList = GetWeatherData(csvWeatherDataFile);

                foreach (var triple in weatherDataList
                    .SelectMany(x => ConvertWeatherData(x, stationNodes)))
                {
                    WriteTriple(streamWriter, triple);
                }
            }
        }

        private static List<Triple> ConvertWeatherData(WeatherDataDto weather, Dictionary<string, WeatherStationDto> stations)
        {
            var triples = new TripleBuilder(nodeFactory.AsBlankNode(weather.NodeId));

            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.Type), nodeFactory.AsValueNode(Constants.Types.WeatherData));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataStation), nodeFactory.AsValueNode(weather.station));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataAlti), nodeFactory.AsValueNode(weather.alti));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataDrct), nodeFactory.AsValueNode(weather.drct));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataDwpc), nodeFactory.AsValueNode(weather.dwpc));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataDwpf), nodeFactory.AsValueNode(weather.dwpf));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataFeelc), nodeFactory.AsValueNode(weather.feelc));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataFeelf), nodeFactory.AsValueNode(weather.feelf));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataGust), nodeFactory.AsValueNode(weather.gust));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataIceAccretion1hr), nodeFactory.AsValueNode(weather.ice_accretion_1hr));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataIceAccretion3hr), nodeFactory.AsValueNode(weather.ice_accretion_3hr));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataIceAccretion6hr), nodeFactory.AsValueNode(weather.ice_accretion_6hr));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataLatitude), nodeFactory.AsValueNode(weather.lat));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataLongitude), nodeFactory.AsValueNode(weather.lon));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataMetar), nodeFactory.AsValueNode(weather.metar));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataMslp), nodeFactory.AsValueNode(weather.mslp));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataP01i), nodeFactory.AsValueNode(weather.p01i));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataPeakWindDrct), nodeFactory.AsValueNode(weather.peak_wind_drct));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataPeakWindGust), nodeFactory.AsValueNode(weather.peak_wind_gust));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataPeakWindTime_hh), nodeFactory.AsValueNode(weather.peak_wind_time_hh));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataPeakWindTime_MM), nodeFactory.AsValueNode(weather.peak_wind_time_MM));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataRelh), nodeFactory.AsValueNode(weather.relh));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSknt), nodeFactory.AsValueNode(weather.sknt));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyc1), nodeFactory.AsValueNode(weather.skyc1));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyc2), nodeFactory.AsValueNode(weather.skyc2));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyc3), nodeFactory.AsValueNode(weather.skyc3));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyc4), nodeFactory.AsValueNode(weather.skyc4));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyl1), nodeFactory.AsValueNode(weather.skyl1));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyl2), nodeFactory.AsValueNode(weather.skyl2));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyl3), nodeFactory.AsValueNode(weather.skyl3));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataSkyl4), nodeFactory.AsValueNode(weather.skyl4));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataTimestamp), nodeFactory.AsValueNode(weather.valid));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataTmpc), nodeFactory.AsValueNode(weather.tmpc));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataTmpf), nodeFactory.AsValueNode(weather.tmpf));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataVsbyKm), nodeFactory.AsValueNode(weather.vsby_km));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataVsbyMi), nodeFactory.AsValueNode(weather.vsby_mi));
            triples.Add(nodeFactory.CreateUriNode(Constants.Predicates.WeatherDataWxcodes), nodeFactory.AsValueNode(weather.wxcodes));

            if (stations.TryGetValue(weather.station, out WeatherStationDto station))
            {
                triples.Add(nodeFactory.AsUriNode(Constants.Predicates.HasStation), nodeFactory.AsBlankNode(station.NodeId));
            }

            return triples.Build();
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
                    NodeId = $"carrier_{nodeIdGenerator.GetNextId()}",
                    Code = x.Code,
                    Description = x.Description
                });
        }

        private static ParallelQuery<AircraftDto> GetAircraftData(string filename)
        {
            return Csv.Faa.Parser.Parsers.FaaAircraftParser
                // Parse as ASCII file:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only use valid lines:
                .Where(x => x.IsValid)
                // Get the Result:
                .Select(x => x.Result)
                // Get Carrier:
                .Select(x => new AircraftDto
                {
                    NodeId = $"aircraft_{nodeIdGenerator.GetNextId()}",
                    AircraftManufacturer = x.AircraftManufacturer,
                    AircraftModel = x.AircraftModel,
                    AircraftSeats = x.AircraftSeats,
                    EngineHorsepower = x.EngineHorsepower,
                    EngineManufacturer = x.EngineManufacturer,
                    EngineModel = x.EngineModel,
                    EngineThrust = x.EngineThrust,
                    N_Number = x.N_Number?.Trim('N'),
                    SerialNumber = x.SerialNumber,
                    UniqueId = x.UniqueId
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
                    NodeId = $"airport_{nodeIdGenerator.GetNextId()}",
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
                    NodeId = $"flight_{nodeIdGenerator.GetNextId()}",
                    FlightNumber = x.FlightNumber,
                    Year = x.Year,
                    Month = x.Month,
                    DayOfMonth = x.DayOfMonth,
                    DayOfWeek = x.DayOfWeek,
                    Carrier = x.UniqueCarrier,
                    OriginAirport = x.OriginAirport,
                    DestinationAirport = x.DestinationAirport,
                    TailNumber = x.TailNumber?.Trim('N'),
                    ArrivalDelay = x.ArrivalDelay,
                    CancellationCode = x.CancellationCode,
                    DepartureDelay = x.DepartureDelay,
                    CarrierDelay = x.CarrierDelay,
                    Distance = x.Distance,
                    LateAircraftDelay = x.LateAircraftDelay,
                    NasDelay = x.NasDelay,
                    SecurityDelay = x.SecurityDelay,
                    WeatherDelay = x.WeatherDelay,
                    FlightDate = x.FlightDate,
                    ActualDepartureTime = x.ActualDepartureTime,
                    ScheduledDepartureTime = x.ScheduledDepartureTime
                });
        }

        private static ParallelQuery<WeatherStationDto> GetWeatherStationData(string filename)
        {
            return Csv.Ncar.Parser.Parsers.MetarStationParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Return the Graph Model:
                .Select(x => new WeatherStationDto
                {
                    NodeId = $"station_{nodeIdGenerator.GetNextId()}",
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
            return Csv.Asos.Parser.Parsers.AsosDatasetParser
                // Read from the Master Coordinate CSV File:
                .ReadFromFile(filename, Encoding.ASCII)
                // Only take valid entities:
                .Where(x => x.IsValid)
                // Get the parsed result:
                .Select(x => x.Result)
                // Return the Graph Model:
                .Select(x => new WeatherDataDto
                {
                    NodeId = $"weather_{nodeIdGenerator.GetNextId()}",
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

        #region Utilities

        public class TripleBuilder
        {
            private readonly INode subj;
            private readonly List<Triple> triples;

            public TripleBuilder(INode subj)
            {
                this.subj = subj;
                this.triples = new List<Triple>();
            }

            public TripleBuilder Add(INode pred, INode obj)
            {
                if (obj == null)
                {
                    return this;
                }

                triples.Add(new Triple(subj, pred, obj));

                return this;
            }

            public TripleBuilder AddWithSubject(INode subj, INode pred, INode obj)
            {
                if (obj == null)
                {
                    return this;
                }

                triples.Add(new Triple(subj, pred, obj));

                return this;
            }

            public List<Triple> Build()
            {
                return triples;
            }
        }

        private static void WriteTriple(StreamWriter streamWriter, Triple triple)
        {
            var value = nTriplesFormatter.Format(triple);

            streamWriter.WriteLine(value);
        }

        #endregion

    }
}