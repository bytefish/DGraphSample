//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using DGraphSample.Api;
//using DGraphSample.Api.Client;
//using DGraphSample.Csv.Model;
//using DGraphSample.DGraph.Model;
//using Grpc.Core;

//namespace DGraphSample.DGraph.Client
//{
//    public class DgraphClient
//    {
//        private readonly Dgraph.DgraphClient client;

//        public DgraphClient(string host, int port, ChannelCredentials credentials)
//        {
//            this.client = DGraphClientUtils.GetClient(host, port, credentials);
//        }

//        public void CreateSchema()
//        {
//            string[] queries = {
//                // Indexes:
//                "CREATE INDEX ON :Flight(flight_number)",
//                "CREATE INDEX ON :Airport(abbr)",
//                // Constraints:
//                "CREATE CONSTRAINT ON (a:Airport) ASSERT a.airport_id IS UNIQUE",
//                "CREATE CONSTRAINT ON (r:Reason) ASSERT r.code IS UNIQUE",
//                "CREATE CONSTRAINT ON (c:State) ASSERT c.name IS UNIQUE",
//                "CREATE CONSTRAINT ON (c:City) ASSERT c.name IS UNIQUE",
//                "CREATE CONSTRAINT ON (c:Country) ASSERT c.name IS UNIQUE",
//                "CREATE CONSTRAINT ON (c:Carrier) ASSERT c.code IS UNIQUE",
//                "CREATE CONSTRAINT ON (c:Aircraft) ASSERT c.tail_num IS UNIQUE"
//            };

//            using (var session = driver.Session())
//            {
//                foreach (var query in queries)
//                {
//                    session.Run(query);
//                }
//            }
//        }

//        public void CreateReasons(IList<Reason> reasons)
//        {
//            string cypher = new StringBuilder()
//                .AppendLine("UNWIND {reasons} AS reason")
//                .AppendLine("MERGE (r:Reason {code: reason.code})")
//                .AppendLine("SET r = reason")
//                .ToString();

//        }

//        public void CreateCarriers(IList<Carrier> carriers)
//        {
//            string cypher = new StringBuilder()
//                .AppendLine("UNWIND {carriers} AS carrier")
//                .AppendLine("MERGE (c:Carrier {code: carrier.code})")
//                .AppendLine("SET c = carrier")
//                .ToString();

//            using (var session = driver.Session())
//            {
//                var result = session.WriteTransaction(tx => tx.Run(cypher, new Dictionary<string, object>() { { "carriers", ParameterSerializer.ToDictionary(carriers) } }));

//                // Get the Summary for Diagnostics:
//                var summary = result.Consume();

//                Console.WriteLine($"[{DateTime.Now}] [Carriers] #NodesCreated: {summary.Counters.NodesCreated}, #RelationshipsCreated: {summary.Counters.RelationshipsCreated}");
//            }
//        }

//        public void CreateAirports(IList<AirportInformation> airports)
//        {
//            string cypher = new StringBuilder()
//                .AppendLine("UNWIND {airports} AS row")
//                // Add the Country:
//                .AppendLine("MERGE (country:Country { name: row.country.name })")
//                .AppendLine("SET country = row.country")
//                .AppendLine()
//                // Add the City:
//                .AppendLine("WITH country, row")
//                .AppendLine("MERGE (city:City { name: row.city.name })")
//                .AppendLine("SET city = row.city")
//                .AppendLine("MERGE (city)-[:IN_COUNTRY]->(country)")
//                .AppendLine()
//                // Add the Optional State:
//                .AppendLine("WITH city, country, row")
//                .AppendLine("FOREACH (s IN (CASE row.state.name WHEN \"\" THEN [] ELSE [row.state.name] END) |")
//                .AppendLine("   MERGE (state:State {name: row.state.name})")
//                .AppendLine("   MERGE (state)-[:IN_COUNTRY]->(country)")
//                .AppendLine("   MERGE (city)-[:IN_STATE]->(state)")
//                .AppendLine(")")
//                .AppendLine()
//                // Add the Airport:
//                .AppendLine("WITH city, row")
//                .AppendLine("MERGE (airport:Airport {airport_id: row.airport.airport_id})")
//                .AppendLine("SET airport = row.airport")
//                .AppendLine("MERGE (airport)-[r:IN_CITY]->(city)")
//                .AppendLine()
//                .ToString();

//            using (var session = driver.Session())
//            {
//                var result = session.WriteTransaction(tx => tx.Run(cypher, new Dictionary<string, object>() { { "airports", ParameterSerializer.ToDictionary(airports) } }));

//                // Get the Summary for Diagnostics:
//                var summary = result.Consume();

//                Console.WriteLine($"[{DateTime.Now}] [Airports] #NodesCreated: {summary.Counters.NodesCreated}, #RelationshipsCreated: {summary.Counters.RelationshipsCreated}");
//            }
//        }

//        public void CreateFlights(IList<FlightInformation> flights)
//        {
//            string cypher = new StringBuilder()
//                .AppendLine("UNWIND {flights} AS row")
//                // Get the Airports of this Flight:
//                .AppendLine("MATCH (oAirport:Airport {airport_id: row.origin})")
//                .AppendLine("MATCH (dAirport:Airport {airport_id: row.destination})")
//                // Create the Flight Item:
//                .AppendLine("CREATE (f:Flight {flight_number: row.flight_number}),")
//                // Relate Flight to Origin Airport:
//                .AppendLine("   (f)-[:ORIGIN {taxi_time: row.taxi_out, dep_delay: row.departure_delay}]->(oAirport),")
//                .AppendLine("   (f)-[:DESTINATION {taxi_time: row.taxi_in, arr_delay: row.arrival_delay}]->(dAirport)")
//                .AppendLine()
//                // Set Flight Details:
//                .AppendLine("SET f.year = row.year,")
//                .AppendLine("    f.month = row.month,")
//                .AppendLine("    f.day = row.day_of_month,")
//                .AppendLine("    f.weekday = row.day_of_week")
//                .AppendLine()
//                // Add Carrier Information:
//                .AppendLine("WITH row, f")
//                .AppendLine("MATCH (car:Carrier {code: row.carrier})")
//                .AppendLine("CREATE (f)-[:CARRIER]->(car)")
//                .AppendLine()
//                // Add Cancellation Information:
//                .AppendLine("WITH row, f")
//                .AppendLine("OPTIONAL MATCH (r:Reason {code: row.cancellation_code})")
//                .AppendLine("FOREACH (o IN CASE WHEN r IS NOT NULL THEN [r] ELSE [] END |")
//                .AppendLine("   CREATE (f) -[:CANCELLED_BY]->(r)")
//                .AppendLine(")")
//                .AppendLine()
//                // Add Delay Information:
//                .AppendLine("WITH row, f")
//                .AppendLine("UNWIND (CASE row.delays WHEN [] THEN [null] else row.delays END) as delay")
//                .AppendLine("OPTIONAL MATCH (r:Reason {code: delay.reason})")
//                .AppendLine("FOREACH (o IN CASE WHEN r IS NOT NULL THEN [r] ELSE [] END |")
//                .AppendLine("   CREATE (f)-[fd:DELAYED_BY]->(r)")
//                .AppendLine("   SET fd.delay = delay.duration")
//                .AppendLine(")")
//                .AppendLine()
//                // Add Aircraft Information:
//                .AppendLine("WITH row, f")
//                .AppendLine("FOREACH(a IN (CASE row.tail_number WHEN \"\" THEN [] ELSE [row.tail_number] END) |")
//                .AppendLine("   MERGE (craft:Aircraft {tail_num: a})")
//                .AppendLine("   CREATE (f)-[:AIRCRAFT]->(craft)")
//                .AppendLine(")")
//                .ToString();

//            using (var session = driver.Session())
//            {
//                // Insert in a Transaction:
//                var result = session.WriteTransaction(tx => tx.Run(cypher, new Dictionary<string, object>() { { "flights", ParameterSerializer.ToDictionary(flights) } }));

//                // Get the Summary for Diagnostics:
//                var summary = result.Consume();

//                Console.WriteLine($"[{DateTime.Now}] [Flights] #NodesCreated: {summary.Counters.NodesCreated}, #RelationshipsCreated: {summary.Counters.RelationshipsCreated}");
//            }
//        }

//        private Task RunInTransactionAsync(Api.Request request)
//        {
//            var transactionContext = new TxnContext();

//            try
//            {
//                request.StartTs = transactionContext.StartTs;
//                request.LinRead = transactionContext.LinRead;

//            }
//            finally
//            {
//                transactionContext.;
//            }
//        }

//    }
//}
