// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Exporter.Utils;
using System;

namespace DGraphSample.Exporter
{
    public static class Constants
    {
        public static readonly Uri NsAviationBaseUri = new Uri("http://www.bytefish.de/aviation/");

        // Ontologies:
        public static readonly Uri NsAviationGeneral = new Uri(NsAviationBaseUri, "General#");
        public static readonly Uri NsAviationtAircraft = new Uri(NsAviationBaseUri, "Aircraft#");
        public static readonly Uri NsAviationAirport = new Uri(NsAviationBaseUri, "Airport#");
        public static readonly Uri NsAviationCarrier = new Uri(NsAviationBaseUri, "Carrier#");
        public static readonly Uri NsAviationFlight = new Uri(NsAviationBaseUri, "Flight#");
        public static readonly Uri NsAviationWeatherStation = new Uri(NsAviationBaseUri, "WeatherStation#");
        public static readonly Uri NsAviationWeather = new Uri(NsAviationBaseUri, "Weather#");

        public static class Types
        {
            public const string Aircraft = "Aircraft";
            public const string Flight = "Flight";
            public const string Carrier = "Carrier";
            public const string Airport = "Airport";
            public const string WeatherStation = "Station";
            public const string WeatherData = "Weather";
        }

        public static class Predicates
        {
            public static readonly Uri Type = UriHelper.SetFragment(NsAviationGeneral, "dgraph.type");

            // Airport Data:
            public static readonly Uri AirportId = UriHelper.SetFragment(NsAviationAirport, "airport.airport_id");
            public static readonly Uri AirportName = UriHelper.SetFragment(NsAviationAirport, "airport.name");
            public static readonly Uri AirportIata = UriHelper.SetFragment(NsAviationAirport, "airport.iata");
            public static readonly Uri AirportCity = UriHelper.SetFragment(NsAviationAirport, "airport.city");
            public static readonly Uri AirportState = UriHelper.SetFragment(NsAviationAirport, "airport.state");
            public static readonly Uri AirportCountry = UriHelper.SetFragment(NsAviationAirport, "airport.country");

            // Carrier Data:
            public static readonly Uri CarrierCode = UriHelper.SetFragment(NsAviationCarrier, "carrier.code");
            public static readonly Uri CarrierDescription = UriHelper.SetFragment(NsAviationCarrier, "carrier.description");

            // Flight Data:
            public static readonly Uri FlightTailNumber = UriHelper.SetFragment(NsAviationFlight, "flight.tail_number");
            public static readonly Uri FlightNumber = UriHelper.SetFragment(NsAviationFlight, "flight.flight_number");
            public static readonly Uri FlightDate = UriHelper.SetFragment(NsAviationFlight, "flight.flight_date");
            public static readonly Uri FlightCarrier = UriHelper.SetFragment(NsAviationFlight, "flight.carrier");
            public static readonly Uri FlightYear = UriHelper.SetFragment(NsAviationFlight, "flight.year");
            public static readonly Uri FlightMonth = UriHelper.SetFragment(NsAviationFlight, "flight.month");
            public static readonly Uri FlightDayOfWeek = UriHelper.SetFragment(NsAviationFlight, "flight.day_of_week");
            public static readonly Uri FlightDayOfMonth = UriHelper.SetFragment(NsAviationFlight, "flight.day_of_month");
            public static readonly Uri FlightCancellationCode = UriHelper.SetFragment(NsAviationFlight, "flight.cancellation_code");
            public static readonly Uri FlightDistance = UriHelper.SetFragment(NsAviationFlight, "flight.distance");
            public static readonly Uri FlightDepartureDelay = UriHelper.SetFragment(NsAviationFlight, "flight.departure_delay");
            public static readonly Uri FlightArrivalDelay = UriHelper.SetFragment(NsAviationFlight, "flight.arrival_delay");
            public static readonly Uri FlightCarrierDelay = UriHelper.SetFragment(NsAviationFlight, "flight.carrier_delay");
            public static readonly Uri FlightWeatherDelay = UriHelper.SetFragment(NsAviationFlight, "flight.weather_delay");
            public static readonly Uri FlightNasDelay = UriHelper.SetFragment(NsAviationFlight, "flight.nas_delay");
            public static readonly Uri FlightSecurityDelay = UriHelper.SetFragment(NsAviationFlight, "flight.security_delay");
            public static readonly Uri FlightLateAircraftDelay = UriHelper.SetFragment(NsAviationFlight, "flight.late_aircraft_delay");
            public static readonly Uri FlightScheduledDepartureTime = UriHelper.SetFragment(NsAviationFlight, "flight.scheduled_departure_time");
            public static readonly Uri FlightActualDepartureTime = UriHelper.SetFragment(NsAviationFlight, "flight.actual_departure_time");

            // Aircraft Data:
            public static readonly Uri AircraftN_Number = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.n_number");
            public static readonly Uri AircraftSerialNumber = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.serial_number");
            public static readonly Uri AircraftUniqueId = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.unique_id");
            public static readonly Uri AircraftManufacturer = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.manufacturer");
            public static readonly Uri AircraftModel = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.model");
            public static readonly Uri AircraftSeats = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.seats");
            public static readonly Uri AircraftEngineManufacturer = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.engine_manufacturer");
            public static readonly Uri AircraftEngineModel = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.engine_model");
            public static readonly Uri AircraftEngineHorsepower = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.engine_horsepower");
            public static readonly Uri AircraftEngineThrust = UriHelper.SetFragment(NsAviationtAircraft, "aircraft.engine_thrust");

            // Weather Station Data:
            public static readonly Uri WeatherStationIcao = UriHelper.SetFragment(NsAviationWeatherStation, "station.icao");
            public static readonly Uri WeatherStationName = UriHelper.SetFragment(NsAviationWeatherStation, "station.name");
            public static readonly Uri WeatherStationIata = UriHelper.SetFragment(NsAviationWeatherStation, "station.iata");
            public static readonly Uri WeatherStationSynop = UriHelper.SetFragment(NsAviationWeatherStation, "station.synop");
            public static readonly Uri WeatherStationLat = UriHelper.SetFragment(NsAviationWeatherStation, "station.lat");
            public static readonly Uri WeatherStationLon = UriHelper.SetFragment(NsAviationWeatherStation, "station.lon");
            public static readonly Uri WeatherStationElevation = UriHelper.SetFragment(NsAviationWeatherStation, "station.elevation");

            // Weather Data:
            public static readonly Uri WeatherDataStation = UriHelper.SetFragment(NsAviationWeather, "weather.station");
            public static readonly Uri WeatherDataLongitude = UriHelper.SetFragment(NsAviationWeather, "weather.lon");
            public static readonly Uri WeatherDataLatitude = UriHelper.SetFragment(NsAviationWeather, "weather.lat");
            public static readonly Uri WeatherDataTimestamp = UriHelper.SetFragment(NsAviationWeather, "weather.timestamp");
            public static readonly Uri WeatherDataTmpf = UriHelper.SetFragment(NsAviationWeather, "weather.tmpf");
            public static readonly Uri WeatherDataTmpc = UriHelper.SetFragment(NsAviationWeather, "weather.tmpc");
            public static readonly Uri WeatherDataDwpf = UriHelper.SetFragment(NsAviationWeather, "weather.dwpf");
            public static readonly Uri WeatherDataDwpc = UriHelper.SetFragment(NsAviationWeather, "weather.dwpc");
            public static readonly Uri WeatherDataRelh = UriHelper.SetFragment(NsAviationWeather, "weather.relh");
            public static readonly Uri WeatherDataDrct = UriHelper.SetFragment(NsAviationWeather, "weather.drct");
            public static readonly Uri WeatherDataSknt = UriHelper.SetFragment(NsAviationWeather, "weather.sknt");
            public static readonly Uri WeatherDataP01i = UriHelper.SetFragment(NsAviationWeather, "weather.p01i");
            public static readonly Uri WeatherDataAlti = UriHelper.SetFragment(NsAviationWeather, "weather.alti");
            public static readonly Uri WeatherDataMslp = UriHelper.SetFragment(NsAviationWeather, "weather.mslp");
            public static readonly Uri WeatherDataVsbyMi = UriHelper.SetFragment(NsAviationWeather, "weather.vsby_mi");
            public static readonly Uri WeatherDataVsbyKm = UriHelper.SetFragment(NsAviationWeather, "weather.vsby_km");
            public static readonly Uri WeatherDataGust = UriHelper.SetFragment(NsAviationWeather, "weather.gust");
            public static readonly Uri WeatherDataSkyc1 = UriHelper.SetFragment(NsAviationWeather, "weather.skyc1");
            public static readonly Uri WeatherDataSkyc2 = UriHelper.SetFragment(NsAviationWeather, "weather.skyc2");
            public static readonly Uri WeatherDataSkyc3 = UriHelper.SetFragment(NsAviationWeather, "weather.skyc3");
            public static readonly Uri WeatherDataSkyc4 = UriHelper.SetFragment(NsAviationWeather, "weather.skyc4");
            public static readonly Uri WeatherDataSkyl1 = UriHelper.SetFragment(NsAviationWeather, "weather.skyl1");
            public static readonly Uri WeatherDataSkyl2 = UriHelper.SetFragment(NsAviationWeather, "weather.skyl2");
            public static readonly Uri WeatherDataSkyl3 = UriHelper.SetFragment(NsAviationWeather, "weather.skyl3");
            public static readonly Uri WeatherDataSkyl4 = UriHelper.SetFragment(NsAviationWeather, "weather.skyl4");
            public static readonly Uri WeatherDataWxcodes = UriHelper.SetFragment(NsAviationWeather, "weather.wxcodes");
            public static readonly Uri WeatherDataFeelf = UriHelper.SetFragment(NsAviationWeather, "weather.feelf");
            public static readonly Uri WeatherDataFeelc = UriHelper.SetFragment(NsAviationWeather, "weather.feelc");
            public static readonly Uri WeatherDataIceAccretion1hr = UriHelper.SetFragment(NsAviationWeather, "weather.ice_accretion_1hr");
            public static readonly Uri WeatherDataIceAccretion3hr = UriHelper.SetFragment(NsAviationWeather, "weather.ice_accretion_3hr");
            public static readonly Uri WeatherDataIceAccretion6hr = UriHelper.SetFragment(NsAviationWeather, "weather.ice_accretion_6hr");
            public static readonly Uri WeatherDataPeakWindGust = UriHelper.SetFragment(NsAviationWeather, "weather.peak_wind_gust");
            public static readonly Uri WeatherDataPeakWindDrct = UriHelper.SetFragment(NsAviationWeather, "weather.peak_wind_drct");
            public static readonly Uri WeatherDataPeakWindTime_hh = UriHelper.SetFragment(NsAviationWeather, "weather.peak_wind_time_hh");
            public static readonly Uri WeatherDataPeakWindTime_MM = UriHelper.SetFragment(NsAviationWeather, "weather.peak_wind_time_MM");
            public static readonly Uri WeatherDataMetar = UriHelper.SetFragment(NsAviationWeather, "weather.metar");

            // Relationships:
            public static readonly Uri HasAircraft = UriHelper.SetFragment(NsAviationGeneral, "has_aircraft");
            public static readonly Uri HasOriginAirport = UriHelper.SetFragment(NsAviationGeneral, "has_origin_airport");
            public static readonly Uri HasDestinationAirport = UriHelper.SetFragment(NsAviationGeneral, "has_destination_airport");
            public static readonly Uri HasCarrier = UriHelper.SetFragment(NsAviationGeneral, "has_carrier");
            public static readonly Uri HasWeatherStation = UriHelper.SetFragment(NsAviationGeneral, "has_weather_station");
        }
    }
}
