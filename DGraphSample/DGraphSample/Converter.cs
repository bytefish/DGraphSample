//// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//using System.Collections.Generic;

//using CsvFlightType = DGraphSample.Csv.Model.Flight;
//using CsvCarrierType = DGraphSample.Csv.Model.Carrier;
//using CsvAirportType = DGraphSample.Csv.Model.Airport;
//using DGraphSample.Api;
//using DGraphSample.DGraph;
//using DGraphSample.Api.Utils;

//namespace DGraphSample
//{

//    public class AirportConverter
//    {
//        protected List<NQuad> Convert(string subject, CsvAirportType source)
//        {
//            List<NQuad> nquads = new List<NQuad>();

//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Name, ObjectValue = new Value { StrVal = source.AirportName } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Abbr, ObjectValue = new Value { StrVal = source.AirportAbbr } });

//            return nquads;
//        }

//    }


//    public class CarrierConverter
//    {
//        protected List<NQuad> Convert(string subject, CsvCarrierType source)
//        {
//            List<NQuad> nquads = new List<NQuad>();

//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Name, ObjectValue = new Value { StrVal = source.Code } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Description, ObjectValue = new Value { StrVal = source.Description } });

//            return nquads;
//        }
//    }

//    public class FlightConverter
//    {
//        protected List<NQuad> Convert(string subject, CsvFlightType source)
//        {
//            List<NQuad> nquads = new List<NQuad>();

//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Year, ObjectValue = new Value { IntVal = source.Year } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.Month, ObjectValue = new Value { IntVal = source.Month } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.DayOfMonth, ObjectValue = new Value { IntVal = source.DayOfMonth } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.DayOfWeek, ObjectValue = new Value { IntVal = source.DayOfWeek } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.FlightDate, ObjectValue = new Value { DatetimeVal = TypeConverters.Convert(source.FlightDate) } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.FlightDate, ObjectValue = new Value { DatetimeVal = TypeConverters.Convert(source.FlightDate) } });
//            nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.FlightNumber, ObjectValue = new Value { StrVal = source.FlightNumber } });

//            if (source.DepartureDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.DepartureDelay, ObjectValue = new Value { IntVal = source.DepartureDelay.Value } });
//                }

//            if (source.TaxiOut.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.TaxiOut, ObjectValue = new Value { IntVal = source.TaxiOut.Value } });
//            }

//            if (source.ArrivalDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.ArrivalDelay, ObjectValue = new Value { IntVal = source.ArrivalDelay.Value } });
//            }

//            if (source.TaxiIn.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.TaxiIn, ObjectValue = new Value { IntVal = source.TaxiIn.Value } });
//            }

//            if (source.CarrierDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.CarrierDelay, ObjectValue = new Value { IntVal = source.CarrierDelay.Value } });
//            }

//            if (source.WeatherDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.WeatherDelay, ObjectValue = new Value { IntVal = source.WeatherDelay.Value } });
//            }

//            if (source.NasDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.NasDelay, ObjectValue = new Value { IntVal = source.NasDelay.Value } });
//            }

//            if (source.SecurityDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.SecurityDelay, ObjectValue = new Value { IntVal = source.SecurityDelay.Value } });
//            }

//            if (source.LateAircraftDelay.HasValue)
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.LateAircraftDelay, ObjectValue = new Value { IntVal = source.LateAircraftDelay.Value } });
//            }

//            AddEdges(nquads, subject, source);

//            return nquads;
//        }

//        private void AddEdges(List<NQuad> nquads, string subject, CsvFlightType source)
//        {
//            if (resolver.TryGetUid(Constants.Subjects.Carrier, source.UniqueCarrier, out string carrier_uid))
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.UniqueCarrier, ObjectId = carrier_uid });
//            }

//            if (resolver.TryGetUid(Constants.Subjects.Reason, source.CancellationCode, out string cancellation_reason))
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.CancellationReason, ObjectId = cancellation_reason  });
//            }


//            if (resolver.TryGetUid(Constants.Subjects.Airport, source.OriginAirport, out string origin_airport_uid))
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.OriginAirport, ObjectId = origin_airport_uid });
//            }

//            if (resolver.TryGetUid(Constants.Subjects.Airport, source.DestinationAirport, out string destination_airport_uid))
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.DestinationAirport, ObjectId = destination_airport_uid });
//            }

//            if (resolver.TryGetUid(Constants.Subjects.State, source.OriginState, out string origin_state_uid))
//            {
//                nquads.Add(new NQuad { Subject = subject, Predicate = Constants.Predicates.DestinationAirport, ObjectId = destination_airport_uid });

//            }

//            if (resolver.TryGetUid(Constants.Subjects.State, source.DestinationState, out string destination_state_uid))
//            {

//            }
//        }
//    }
//}
