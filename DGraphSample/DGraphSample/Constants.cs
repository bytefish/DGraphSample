// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DGraphSample.DGraph
{
    public static class Constants
    {
        public static class Types
        {
            public const string Flight = "flight";
            public const string Carrier = "carrier";
            public const string Airport = "airport";
        }

        public static class Predicates
        {
            public const string Type = "node.type";

            // Airport Data:
            public const string AirportName = "airport.name";
            public const string AirportIata = "airport.iata";
            public const string AirportAbbr = "airport.abbr";
            public const string AirportCity = "airport.city";
            public const string AirportState = "airport.state";
            public const string AirportCountry = "airport.country";

            // Weather Station Data:
            public const string WeatherStationIcao = "weather_station.icao";
            public const string WeatherStationName = "weather_station.name";
            public const string WeatherStationIata = "weather_station.iata";

            // 

            // Carrier Data:
            public const string CarrierCode = "carrier.code";
            public const string CarrierDescription = "carrier.description";

            // Flight Data:
            public const string FlightTailNumber = "flight.tail_number";
            public const string FlightNumber = "flight.flight_number";
            public const string FlightDate = "flight.flight_date";
            public const string FlightCarrier = "flight.carrier";
            public const string FlightYear = "flight.year";
            public const string FlightMonth = "flight.month";
            public const string FlightDayOfWeek = "flight.day_of_week";
            public const string FlightDayOfMonth = "flight.day_of_month";
            public const string FlightCancellationCode = "flight.cancellation_code";            
            public const string FlightDistance = "flight.distance";
            public const string FlightDepartureDelay = "flight.departure_delay";
            public const string FlightArrivalDelay = "flight.arrival_delay";
            public const string FlightCarrierDelay = "flight.carrier_delay";
            public const string FlightWeatherDelay = "flight.weather_delay";
            public const string FlightNasDelay = "flight.nas_delay";
            public const string FlightSecurityDelay = "flight.security_delay";
            public const string FlightLateAircraftDelay = "flight.late_aircraft_delay";

            // Relationships:
            public const string HasOriginAirport = "has_origin_airport";
            public const string HasDestinationAirport = "has_destination_airport";
            public const string HasCarrier = "has_carrier";
            public const string HasWeatherStation = "has_weather_station";
        }
    }
}
