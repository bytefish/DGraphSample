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

#### Results ####

```json
{
  "data": {
    "flights": [
      {
        "flight.year": 2014,
        "flight.cancellation_code": "",
        "flight.departure_delay": 11,
        "flight.arrival_delay": -1,
        "flight.flight_date": "2014-03-18T00:00:00Z",
        "flight.day_of_week": 2,
        "flight.distance": 280,
        "flight.actual_departure_time": "2014-03-18T17:26:00Z",
        "flight.tail_number": "965UW",
        "flight.month": 3,
        "has_aircraft": {
          "aircraft.seats": "20",
          "aircraft.engine_thrust": "18820",
          "aircraft.n_number": "965UW",
          "aircraft.manufacturer": "EMBRAER",
          "aircraft.model": "ERJ 190-100 IGW",
          "aircraft.engine_model": "CF34-10E6",
          "aircraft.engine_horsepower": "0",
          "aircraft.serial_number": "19000198",
          "aircraft.unique_id": "1008724",
          "aircraft.engine_manufacturer": "GE"
        },
        "has_destination_airport": {
          "airport.state": "Massachusetts",
          "airport.country": "United States",
          "airport.airport_id": "10721",
          "airport.name": "Logan International",
          "airport.iata": "BOS",
          "airport.city": "Boston, MA"
        },
        "flight.flight_number": "1981",
        "flight.day_of_month": 18,
        "has_origin_airport": {
          "airport.city": "Philadelphia, PA",
          "airport.state": "Pennsylvania",
          "airport.country": "United States",
          "airport.airport_id": "14100",
          "airport.name": "Philadelphia International",
          "airport.iata": "PHL"
        },
        "has_carrier": {
          "carrier.code": "US",
          "carrier.description": "US Airways Inc."
        },
        "flight.scheduled_departure_time": "2014-03-18T17:15:00Z"
      }
    ]
  },
  "extensions": {
    "server_latency": {
      "processing_ns": 6064371600,
      "assign_timestamp_ns": 1000100,
      "total_ns": 6065371700
    },
    "txn": {
      "start_ts": 10067
    },
    "metrics": {
      "num_uids": {
        "": 5819812,
        "aircraft.engine_horsepower": 1,
        "aircraft.engine_manufacturer": 1,
        "aircraft.engine_model": 1,
        "aircraft.engine_thrust": 1,
        "aircraft.manufacturer": 1,
        "aircraft.model": 1,
        "aircraft.n_number": 1,
        "aircraft.seats": 1,
        "aircraft.serial_number": 1,
        "aircraft.unique_id": 1,
        "airport.airport_id": 2,
        "airport.city": 2,
        "airport.code": 2,
        "airport.country": 2,
        "airport.iata": 2,
        "airport.name": 2,
        "airport.state": 2,
        "carrier.code": 1,
        "carrier.description": 1,
        "dgraph.type": 0,
        "flight.actual_departure_time": 1,
        "flight.arrival_delay": 1,
        "flight.cancellation_code": 1,
        "flight.carrier": 1,
        "flight.carrier_delay": 1,
        "flight.day_of_month": 1,
        "flight.day_of_week": 1,
        "flight.departure_delay": 1,
        "flight.distance": 1,
        "flight.flight_date": 5819812,
        "flight.flight_number": 5819812,
        "flight.late_aircraft_delay": 1,
        "flight.month": 1,
        "flight.nas_delay": 1,
        "flight.scheduled_departure_time": 1,
        "flight.security_delay": 1,
        "flight.tail_number": 5819812,
        "flight.weather_delay": 1,
        "flight.year": 1,
        "has_aircraft": 1,
        "has_carrier": 1,
        "has_destination_airport": 1,
        "has_origin_airport": 1,
        "has_weather_station": 2
      }
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

#### Results ####

```json
{
  "data": {
    "q": [
      {
        "uid": "0x121f16",
        "actual_departure": "2014-03-18T17:26:00Z",
        "scheduled_departure": "2014-03-18T17:15:00Z",
        "carrier": {
          "code": "US",
          "description": "US Airways Inc."
        },
        "destination": {
          "uid": "0x246d5",
          "name": "Logan International",
          "weather_station": [
            {
              "measurements": [
                {
                  "timestamp": "2014-03-18T00:54:00Z",
                  "temperature": -2.8
                },
                {
                  "timestamp": "2014-03-18T01:54:00Z",
                  "temperature": -3.3
                },
                {
                  "timestamp": "2014-03-18T02:54:00Z",
                  "temperature": -3.3
                },
                {
                  "timestamp": "2014-03-18T03:54:00Z",
                  "temperature": -3.3
                },
                {
                  "timestamp": "2014-03-18T04:54:00Z",
                  "temperature": -4.4
                },
                {
                  "timestamp": "2014-03-18T05:54:00Z",
                  "temperature": -5
                },
                {
                  "timestamp": "2014-03-18T06:54:00Z",
                  "temperature": -5.6
                },
                {
                  "timestamp": "2014-03-18T07:54:00Z",
                  "temperature": -6.1
                },
                {
                  "timestamp": "2014-03-18T08:54:00Z",
                  "temperature": -6.699999
                },
                {
                  "timestamp": "2014-03-18T09:54:00Z",
                  "temperature": -6.699999
                },
                {
                  "timestamp": "2014-03-18T10:54:00Z",
                  "temperature": -6.699999
                },
                {
                  "timestamp": "2014-03-18T11:54:00Z",
                  "temperature": -6.1
                },
                {
                  "timestamp": "2014-03-18T12:54:00Z",
                  "temperature": -3.3
                },
                {
                  "timestamp": "2014-03-18T13:54:00Z",
                  "temperature": -1.7
                },
                {
                  "timestamp": "2014-03-18T14:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T15:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T16:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T17:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T18:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T19:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T20:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T21:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T22:54:00Z",
                  "temperature": -1.1
                },
                {
                  "timestamp": "2014-03-18T23:54:00Z",
                  "temperature": -1.1
                }
              ]
            }
          ]
        },
        "origin": {
          "uid": "0xc078",
          "name": "Philadelphia International",
          "weather_station": [
            {
              "measurements": [
                {
                  "timestamp": "2014-03-18T00:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T01:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T02:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T03:54:00Z",
                  "temperature": -1.1
                },
                {
                  "timestamp": "2014-03-18T04:54:00Z",
                  "temperature": -1.1
                },
                {
                  "timestamp": "2014-03-18T05:54:00Z",
                  "temperature": -1.1
                },
                {
                  "timestamp": "2014-03-18T06:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T07:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T08:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T09:54:00Z",
                  "temperature": -0.6
                },
                {
                  "timestamp": "2014-03-18T10:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T11:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T12:54:00Z",
                  "temperature": 0
                },
                {
                  "timestamp": "2014-03-18T13:54:00Z",
                  "temperature": 0.600001
                },
                {
                  "timestamp": "2014-03-18T14:54:00Z",
                  "temperature": 1.700001
                },
                {
                  "timestamp": "2014-03-18T15:54:00Z",
                  "temperature": 4.399999
                },
                {
                  "timestamp": "2014-03-18T16:54:00Z",
                  "temperature": 5.600001
                },
                {
                  "timestamp": "2014-03-18T17:54:00Z",
                  "temperature": 6.700001
                },
                {
                  "timestamp": "2014-03-18T18:54:00Z",
                  "temperature": 6.700001
                },
                {
                  "timestamp": "2014-03-18T19:54:00Z",
                  "temperature": 7.199999
                },
                {
                  "timestamp": "2014-03-18T20:54:00Z",
                  "temperature": 7.800001
                },
                {
                  "timestamp": "2014-03-18T21:54:00Z",
                  "temperature": 6.700001
                },
                {
                  "timestamp": "2014-03-18T22:54:00Z",
                  "temperature": 4.399999
                },
                {
                  "timestamp": "2014-03-18T23:54:00Z",
                  "temperature": 3.299999
                }
              ]
            }
          ]
        }
      }
    ]
  },
  "extensions": {
    "server_latency": {
      "processing_ns": 6394680200,
      "total_ns": 6395676700
    },
    "txn": {
      "start_ts": 10066
    },
    "metrics": {
      "num_uids": {
        "": 5840602,
        "airport.name": 2,
        "carrier.code": 1,
        "carrier.description": 1,
        "dgraph.type": 0,
        "flight.actual_departure_time": 1,
        "flight.flight_date": 5819811,
        "flight.flight_number": 5819811,
        "flight.scheduled_departure_time": 1,
        "flight.tail_number": 5819811,
        "has_carrier": 1,
        "has_destination_airport": 1,
        "has_origin_airport": 1,
        "has_weather_station": 2,
        "uid": 3,
        "weather.timestamp": 41628,
        "weather.tmpc": 48,
        "~has_station": 2
      }
    }
  }
}
```

### TOP 10 Airports with Flight Cancellations due to Weather ###

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


#### Results ####

```json
{
  "data": {
    "q": [
      {
        "uid": "0x4b7bd",
        "airport": "Chicago O'Hare International",
        "percent_cancelled": 2.180214
      },
      {
        "uid": "0xc05a",
        "airport": "LaGuardia",
        "percent_cancelled": 1.858534
      },
      {
        "uid": "0x2bb89",
        "airport": "Ronald Reagan Washington National",
        "percent_cancelled": 1.570493
      },
      {
        "uid": "0x3cdd7",
        "airport": "Dallas/Fort Worth International",
        "percent_cancelled": 1.521331
      },
      {
        "uid": "0x1f89e",
        "airport": "Chicago Midway International",
        "percent_cancelled": 1.352527
      },
      {
        "uid": "0x35819",
        "airport": "Newark Liberty International",
        "percent_cancelled": 1.341114
      },
      {
        "uid": "0x3cdef",
        "airport": "Washington Dulles International",
        "percent_cancelled": 1.256519
      },
      {
        "uid": "0x37f82",
        "airport": "Hartsfield-Jackson Atlanta International",
        "percent_cancelled": 1.206396
      },
      {
        "uid": "0x1d15f",
        "airport": "George Bush Intercontinental/Houston",
        "percent_cancelled": 1.181907
      },
      {
        "uid": "0x3587b",
        "airport": "San Francisco International",
        "percent_cancelled": 1.160025
      }
    ]
  },
  "extensions": {
    "server_latency": {
      "processing_ns": 872977700,
      "assign_timestamp_ns": 999900,
      "total_ns": 873977600
    },
    "txn": {
      "start_ts": 10065
    },
    "metrics": {
      "num_uids": {
        "": 0,
        "airport.name": 10,
        "dgraph.type": 0,
        "flight.cancellation_code": 5819811,
        "total_flights": 325,
        "uid": 985,
        "~has_origin_airport": 1300
      }
    }
  }
}
```


### TOP 10 Airports for Flight Cancellations ###

```
{
  var(func: type(Airport)) @filter(has(~has_origin_airport)) {
    uid
    total_flights as count(~has_origin_airport)
  }
    
  var(func: type(Airport)) @filter(has(~has_origin_airport)) {
    uid
    cancelled_flights as count(~has_origin_airport) @filter(
      eq(flight.cancellation_code, "A") 
      	or eq(flight.cancellation_code, "B") 
      	or eq(flight.cancellation_code, "C")
    		or eq(flight.cancellation_code, "D"))
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

#### Result #### 

```json
{
  "data": {
    "q": [
      {
        "uid": "0x4b7bd",
        "airport": "Chicago O'Hare International",
        "percent_cancelled": 4.687217
      },
      {
        "uid": "0xc05a",
        "airport": "LaGuardia",
        "percent_cancelled": 4.367743
      },
      {
        "uid": "0x35819",
        "airport": "Newark Liberty International",
        "percent_cancelled": 4.362246
      },
      {
        "uid": "0x3cdef",
        "airport": "Washington Dulles International",
        "percent_cancelled": 3.497599
      },
      {
        "uid": "0x2bb89",
        "airport": "Ronald Reagan Washington National",
        "percent_cancelled": 3.128576
      },
      {
        "uid": "0x1f89e",
        "airport": "Chicago Midway International",
        "percent_cancelled": 2.848555
      },
      {
        "uid": "0x3587b",
        "airport": "San Francisco International",
        "percent_cancelled": 2.550736
      },
      {
        "uid": "0x3cdd7",
        "airport": "Dallas/Fort Worth International",
        "percent_cancelled": 2.454107
      },
      {
        "uid": "0x469a4",
        "airport": "John F. Kennedy International",
        "percent_cancelled": 2.306086
      },
      {
        "uid": "0x49118",
        "airport": "Nashville International",
        "percent_cancelled": 2.274115
      }
    ]
  },
  "extensions": {
    "server_latency": {
      "processing_ns": 882973700,
      "total_ns": 882973700
    },
    "txn": {
      "start_ts": 10063
    },
    "metrics": {
      "num_uids": {
        "": 17459433,
        "airport.name": 10,
        "dgraph.type": 0,
        "flight.cancellation_code": 23279244,
        "total_flights": 325,
        "uid": 985,
        "~has_origin_airport": 1300
      }
    }
  }
}
```

### Airports with a Weather Station ###

```graphql
{
  q(func: type(Airport), first: 5) @filter(has(has_weather_station)) {
    airport.airport_id
    airport.name
    has_weather_station {
      station.name
      station.elevation
    }
  }
}
```

#### Results ####

```json
{
  "data": {
    "q": [
      {
        "airport.airport_id": "10234",
        "airport.name": "Arvidsjaur Airport",
        "has_weather_station": [
          {
            "station.name": "CORNELIA",
            "station.elevation": 441
          }
        ]
      },
      {
        "airport.airport_id": "10247",
        "airport.name": "Anaktuvuk Pass Airport",
        "has_weather_station": [
          {
            "station.name": "ANAKTUVUK PASS",
            "station.elevation": 642
          }
        ]
      },
      {
        "airport.airport_id": "10276",
        "airport.name": "Thomas C. Russell Field",
        "has_weather_station": [
          {
            "station.name": "ALEXANDER/RUSSEL",
            "station.elevation": 209
          }
        ]
      },
      {
        "airport.airport_id": "10317",
        "airport.name": "Lima Allen County",
        "has_weather_station": [
          {
            "station.name": "LIMA",
            "station.elevation": 296
          }
        ]
      },
      {
        "airport.airport_id": "10376",
        "airport.name": "Amami",
        "has_weather_station": [
          {
            "station.name": "AHOSKIE/TRI COUN",
            "station.elevation": 21
          }
        ]
      }
    ]
  },
  "extensions": {
    "server_latency": {
      "processing_ns": 16999100,
      "total_ns": 16999100
    },
    "txn": {
      "start_ts": 10068
    },
    "metrics": {
      "num_uids": {
        "airport.airport_id": 5,
        "airport.name": 5,
        "dgraph.type": 0,
        "has_weather_station": 10,
        "station.elevation": 5,
        "station.name": 5
      }
    }
  }
}
```

## Resources ##

* [Jepsen: Dgraph 1.0.2](https://jepsen.io/analyses/dgraph-1-0-2)
    * An in-depth analysis of Dgraph 1.0.2. Explains a lot of Dgraph concepts and internals.

[Dgraph]: https://dgraph.io/
