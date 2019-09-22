// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace DGraphSample.DGraph.Queries
{
    public static class Query
    {
        public static string Schema
        {
            get
            {
                return File.ReadAllText("Resources/schema.txt");
            }
        }

        public static string GetAllAirports
        {
            get
            {
                return File.ReadAllText("Resources/get_all_airports.txt");
            }
        }

        public static string GetAllCarriers
        {
            get
            {
                return File.ReadAllText("Resources/get_all_carriers.txt");
            }
        }
    }
}
