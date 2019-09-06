# DGraphSample #

*Please Note: This repository is a work in progress.*

I have written about working with Neo4j and the SQL Server Graph Database:

* [https://bytefish.de/blog/neo4j_at_scale_airline_dataset/](https://bytefish.de/blog/neo4j_at_scale_airline_dataset/)
* [https://bytefish.de/blog/sql_server_2017_graph_database/](https://bytefish.de/blog/sql_server_2017_graph_database/)

In this repository I want to analyze the Airline On Time Performance dataset using [Dgraph]:

> Dgraph is an open source, scalable, distributed, highly available and fast graph database, 
> designed from ground up to be run in production.

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

[Dgraph]: https://dgraph.io/
