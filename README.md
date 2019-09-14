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
type: string @index(exact) .
flight_number: string @index(exact) .
name: string @index(exact) .
airport_id: string @index(exact) .
abbr: string .
code: string .
description: string .
year: int .
month: int .
day_of_month: int .
day_of_week: int .
flight_date: dateTime .
tail_number: string .
cancellation_code: string .
origin_airport: uid @reverse .
destination_airport: uid @reverse .
distance: float .
carrier: uid @reverse .
city: string @index(exact) .
state: string @index(exact) .
country: string @index(exact) .
departure_delay: int .
arrival_delay: int .
carrier_delay: int .
weather_delay: int .
nas_delay: int .
security_delay: int .
late_aircraft_delay: int .
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
