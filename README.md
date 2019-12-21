# DGraphSample #

*Please Note: This repository is a work in progress.*

In this repository I want to analyze the Airline On Time Performance dataset using [Dgraph]:

> Dgraph is an open source, scalable, distributed, highly available and fast graph database, 
> designed from ground up to be run in production.

I have written about working with Neo4j and the SQL Server Graph Database:

* [https://bytefish.de/blog/neo4j_at_scale_airline_dataset/](https://bytefish.de/blog/neo4j_at_scale_airline_dataset/)
* [https://bytefish.de/blog/sql_server_2017_graph_database/](https://bytefish.de/blog/sql_server_2017_graph_database/)

## Starting Dgraph ##

Starting Dgraph consists of running **Dgraph Zero** and **Dgraph Alpha**:

* **Dgraph Zero**
    * Controls the Dgraph cluster, assigns servers to a group and re-balances data between server groups.
* **Dgraph Alpha** 
    * Hosts predicates and indexes.

So first start **Dgraph Zero** by running:

```
dgraph zero
```

Then start **Dgraph Alpha** by running:

```
dgraph alpha --lru_mb 4096 --zero localhost:5080
```

## Dgraph Schema ##

```
#
# General Node Data
#
node_type: string @index(exact) .

#
# Aircraft Data
#
aircraft.n_number: string @index(exact) .
aircraft.serial_number: string .
aircraft.unique_id: string .
aircraft.manufacturer: string .
aircraft.model: string .
aircraft.seats: string .
aircraft.engine_manufacturer: string .
aircraft.engine_model: string .
aircraft.engine_horsepower: string .
aircraft.engine_thrust: string .

#
# Airport Data
#
airport.airport_id: string .
airport.name: string @index(exact) .
airport.iata: string @index(exact) .
airport.code: string .
airport.city: string .
airport.state: string .
airport.country: string .

#
# Carrier Data
#
carrier.code: string @index(exact) .
carrier.description: string .

# 
# Weather Station Data
# 
station.icao: string .
station.name: string @index(exact) .
station.iata: string @index(exact) .
station.synop: string .
station.lat: string .
station.lon: string .
station.elevation: float .

#
# METAR or ASOS Weather Measurements
#
weather.timestamp: dateTime .
weather.tmpf: float .
weather.tmpc: float .
weather.dwpf: float .
weather.dwpc: float .
weather.relh: float .
weather.drct: float .
weather.sknt: float .
weather.p01i: float .
weather.alti: float .
weather.mslp: float .
weather.vsby_mi: float .
weather.vsby_km: float .
weather.skyc1: string .
weather.skyc2: string .
weather.skyc3: string .
weather.skyc4: string .
weather.skyl1: float .
weather.skyl2: float .
weather.skyl3: float .
weather.skyl4: float .
weather.wxcodes: string .
weather.feelf: float .
weather.feelc: float .
weather.ice_accretion_1hr: float .
weather.ice_accretion_3hr: float .
weather.ice_accretion_6hr: float .
weather.peak_wind_gust: float .
weather.peak_wind_drct: float .
weather.peak_wind_time_hh: int .
weather.peak_wind_time_MM: int .
weather.metar: string .

#
# Flight Data
#
flight.tail_number: string @index(exact) .
flight.flight_number: string @index(exact) .
flight.flight_date: dateTime .
flight.carrier: string .
flight.year: int .
flight.month: int .
flight.day_of_week: int .
flight.day_of_month: int .
flight.cancellation_code: string @index(exact) .
flight.distance: float .
flight.departure_delay: int .
flight.arrival_delay: int .
flight.carrier_delay: int .
flight.weather_delay: int .
flight.nas_delay: int .
flight.security_delay: int .
flight.late_aircraft_delay: int .
flight.scheduled_departure_time: dateTime .
flight.actual_departure_time: dateTime .

#
# Relationships in Data
#
has_aircraft: uid @reverse .
has_origin_airport: uid @reverse .
has_destination_airport: uid @reverse .
has_carrier: uid @reverse .
has_weather_station: uid @reverse .
has_station: uid @reverse .
```

## Queries ##

### Number of Flights Departed by Airport ###

```graphql
{
   number_of_flights_started(func: eq(type, "airport")) {
    name
    count(~origin_airport)
  }
}
```

## Resources ##

* [Jepsen: Dgraph 1.0.2](https://jepsen.io/analyses/dgraph-1-0-2)
    * An in-depth analysis of Dgraph 1.0.2. Explains a lot of Dgraph concepts and internals.

[Dgraph]: https://dgraph.io/
