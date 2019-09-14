﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DGraphSample.DGraph.Queries
{
    public static class Query
    {
        public const string Schema = @"type: string @index(exact) .
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
                                       late_aircraft_delay: int .";

        public const string GetAllAirports = @"{
                                                 airports(func: eq(type, ""airport"")) {
                                                   uid
                                                   airport_id
                                                   name
                                                   abbr
                                                   city
                                                   state
                                                   country
                                                   }
                                               }";

        public const string GetAllCarriers = @"{
                                                 carriers(func: eq(type, ""carrier"")) {
                                                   uid
                                                   name
                                                   code
                                                   }
                                               }";

    }
}