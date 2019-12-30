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

## Importing the Dataset ##

Dgraph is able to constantly map 177.4k nquads per second during the entire import:

```
[18:13:19+0100] MAP 01h03m20s nquad_count:586.6M err_count:0.000 nquad_speed:154.4k/sec edge_count:674.1M edge_speed:177.4k/sec
```

Importing the entire dataset takes 2h18m14s:

```
[19:28:13+0100] REDUCE 02h18m14s 100.00% edge_count:1.113G edge_speed:532.0k/sec plist_count:991.8M plist_speed:474.2k/sec
Total: 02h18m14s
```

The final ``p`` directory in the ``out`` folder has a size of 19.5 GB.

## Queries ##

### Get all reachable Nodes for a given Flight ###

```graphql
{
  flights(func: type(Flight)) @filter(eq(flight.tail_number, "965UW") and eq(flight.flight_number, "1981") and eq(flight.flight_date, "2014-03-18T00:00:00")) {
      expand(_all_) {
        expand(_all_)
    }
  }
}
```

### Weather for Day of Flight ###

```
{
  q(func: type(Flight)) @filter(eq(flight.tail_number, "965UW") and eq(flight.flight_number, "1981") and eq(flight.flight_date, "2014-03-18T00:00:00")) @cascade {
    uid
    actual_departure: flight.actual_departure_time
    scheduled_departure: flight.scheduled_departure_time
    carrier: has_carrier {
      code: carrier.code
      description: carrier.description
    }
    destination: has_destination_airport {
      uid
      name: airport.name
      weather_station: has_weather_station {
        measurements: ~has_station (orderasc: weather.timestamp) @filter(ge(weather.timestamp, "2014-03-18T00:00:00") and le(weather.timestamp, "2014-03-19T00:00:00")) {
          timestamp: weather.timestamp
          temperature: weather.tmpc
        }
      }
    }
    origin: has_origin_airport {
    uid
    name: airport.name
    weather_station: has_weather_station {
        measurements: ~has_station (orderasc: weather.timestamp) @filter(ge(weather.timestamp, "2014-03-18T00:00:00") and le(weather.timestamp, "2014-03-19T00:00:00")) {
          timestamp: weather.timestamp
          temperature: weather.tmpc
        }
      }
    }
  }
}
```

### Flights Cancelled due to Weather ###

```graphql
{
  var(func: type(Airport)) @filter(has(~has_origin_airport)) {
    uid
    total_flights as count(~has_origin_airport)
  }
    
  var(func: type(Airport)) @filter(has(~has_origin_airport)) {
    uid
    cancelled_flights as count(~has_origin_airport) @filter(eq(flight.cancellation_code, "B"))
  }
      
  var(func: uid(total_flights, cancelled_flights))  {
    uid
    percent_cancelled as math(cancelled_flights / (total_flights * 1.0) * 100.0)
  }

  q(func: uid(percent_cancelled), first: 10, orderdesc: val(percent_cancelled)) @filter(ge(val(total_flights), 50000)) {
    uid
    airport: airport.name
    percent_cancelled: val(percent_cancelled)
  }
}
```

### Airports with a Weather Station ###

```graphql
{
  q(func: type(Airport)) @filter(has(has_weather_station)) {
    airport.airport_id
    airport.name
    has_weather_station {
      station.name
      station.elevation
    }
  }
}
```

## Resources ##

* [Jepsen: Dgraph 1.0.2](https://jepsen.io/analyses/dgraph-1-0-2)
    * An in-depth analysis of Dgraph 1.0.2. Explains a lot of Dgraph concepts and internals.

[Dgraph]: https://dgraph.io/
