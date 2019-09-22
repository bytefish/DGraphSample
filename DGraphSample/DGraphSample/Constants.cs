// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DGraphSample
{
    public static class Constants
    {
        public static class Types
        {
            public const string Flight = "flight";
            public const string Carrier = "carrier";
            public const string Airport = "airport";
            public const string WeatherStation = "weather_station";
            public const string WeatherData = "weather_data";
        }

        public static class Predicates
        {
            public const string Type = "node.type";

            // Airport Data:
            public const string AirportId = "airport.airport_id";
            public const string AirportName = "airport.name";
            public const string AirportIata = "airport.iata";
            public const string AirportCity = "airport.city";
            public const string AirportState = "airport.state";
            public const string AirportCountry = "airport.country";

            // Weather Station Data:
            public const string WeatherStationIcao = "weather_station.icao";
            public const string WeatherStationName = "weather_station.name";
            public const string WeatherStationIata = "weather_station.iata";
            public const string WeatherStationSynop = "weather_station.synop";
            public const string WeatherStationLat = "weather_station.lat";
            public const string WeatherStationLon = "weather_station.lon";
            public const string WeatherStationElevation = "weather_station.elevation";

            // Weather Data:
            /// <summary>
            /// Three or four character site identifier
            /// </summary>
            public const string WeatherDataStation = "weather_data.station";
            public const string WeatherDataLongitude = "weather_data.lon";
            public const string WeatherDataLatitude = "weather_data.lat";
            public const string WeatherDataTimestamp = "weather_data.timestamp";
            public const string WeatherDataTmpf = "weather_data.tmpf";
            public const string WeatherDataTmpc = "weather_data.tmpc";
            public const string WeatherDataDwpf = "weather_data.dwpf";
            public const string WeatherDataDwpc = "weather_data.dwpc";
            public const string WeatherDataRelh = "weather_data.relh";
            public const string WeatherDataDrct = "weather_data.drct";
            public const string WeatherDataSknt = "weather_data.sknt";
            public const string WeatherDataP01i = "weather_data.p01i";
            public const string WeatherDataAlti = "weather_data.alti";
            public const string WeatherDataMslp = "weather_data.mslp";
            public const string WeatherDataVsbyMi = "weather_data.vsby_mi";
            public const string WeatherDataVsbyKm = "weather_data.vsby_km";
            public const string WeatherDataGust = "weather_data.gust";
            public const string WeatherDataSkyc1 = "weather_data.skyc1";
            public const string WeatherDataSkyc2 = "weather_data.skyc2";
            public const string WeatherDataSkyc3 = "weather_data.skyc3";
            public const string WeatherDataSkyc4 = "weather_data.skyc4";
            public const string WeatherDataSkyl1 = "weather_data.skyl1";
            public const string WeatherDataSkyl2 = "weather_data.skyl2";
            public const string WeatherDataSkyl3 = "weather_data.skyl3";
            public const string WeatherDataSkyl4 = "weather_data.skyl4";
            public const string WeatherDataWxcodes = "weather_data.wxcodes";
            public const string WeatherDataFeelf = "weather_data.feelf";
            public const string WeatherDataFeelc = "weather_data.feelc";
            public const string WeatherDataIceAccretion1hr = "weather_data.ice_accretion_1hr";
            public const string WeatherDataIceAccretion3hr = "weather_data.ice_accretion_3hr";
            public const string WeatherDataIceAccretion6hr = "weather_data.ice_accretion_6hr";
            public const string WeatherDataPeakWindGust = "weather_data.peak_wind_gust";
            public const string WeatherDataPeakWindDrct = "weather_data.peak_wind_drct";
            public const string WeatherDataPeakWindTime_hh = "weather_data.peak_wind_time_hh";
            public const string WeatherDataPeakWindTime_MM = "weather_data.peak_wind_time_MM";
            public const string WeatherDataMetar = "weather_data.metar";

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
