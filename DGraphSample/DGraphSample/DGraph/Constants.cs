﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DGraphSample.DGraph
{
    public static class Constants
    {
        public static class Subjects
        {
            public const string Carrier = "Carrier";
            public const string City = "City";
            public const string State = "State";
            public const string Country = "Country";
            public const string Flight = "Flight";
            public const string Reason = "Reason";
            public const string Airport = "Airport";
            public const string Aircraft = "Aircraft";
        }

        public static class Predicates
        {
            public const string Name = "name";
            public const string Abbr = "abbr";
            public const string Code = "code";
            public const string Description = "description";
            public const string Year = "year";
            public const string Month = "month";
            public const string DayOfMonth = "day_of_month";
            public const string DayOfWeek = "day_of_week";
            public const string FlightDate = "flight_date";
            public const string UniqueCarrier = "unique_carrier";
            public const string TailNumber = "tail_number";
            public const string FlightNumber = "flight_number";
            public const string OriginAirport = "origin_airport";
            public const string DestinationAirport = "destination_airport";
            public const string DestinationState = "destination_state";
            public const string DepartureDelay = "departure_delay";
            public const string TaxiOut = "taxi_out";
            public const string TaxiIn = "Taxi_in";
            public const string ArrivalDelay = "arrival_delay";
            public const string CancellationCode = "cancellation_code";
            public const string CarrierDelay = "carrier_delay";
            public const string WeatherDelay = "weather_delay";
            public const string NasDelay = "nas_delay";
            public const string SecurityDelay = "security_delay";
            public const string LateAircraftDelay = "late_aircraft_delay";
            public const string CancellationReason = "cancellation_reason";

            public const string in_city = "in_city";
            public const string in_state = "in_state";
            public const string in_country = "in_country";

        }
    }
}
