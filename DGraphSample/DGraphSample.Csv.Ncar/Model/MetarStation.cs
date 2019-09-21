// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Ncar.Model.Enums;
using System;

namespace DGraphSample.Csv.Ncar.Model
{

    /// <summary>
    public class MetarStation
    {
        /// <summary>
        /// 2 letter state (province) abbreviation
        /// </summary>
        public string CD { get; set; }

        /// <summary>
        /// 16 character station long name
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// 4-character international id
        /// </summary>
        public string ICAO { get; set; }

        /// <summary>
        /// 3-character(FAA) id
        /// </summary>
        public string IATA { get; set; }

        /// <summary>
        /// 5-digit international synoptic number
        /// </summary>
        public string SYNOP { get; set; }

        /// <summary>
        /// Latitude(degrees minutes)
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Longitude(degree minutes)
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Station elevation(meters)
        /// </summary>
        public float? Elevation { get; set; }

        /// <summary>
        /// METAR reporting station. 
        /// </summary>
        public MetarFlagEnum M { get; set; }

        /// <summary>
        /// NEXRAD (WSR-88D) Radar site
        /// </summary>
        public NexradFlagEnum N { get; set; }

        /// <summary>
        /// Aviation-specific flag 
        /// </summary>
        public AviationFlagEnum V { get; set; }

        /// <summary>
        /// Upper air
        /// </summary>
        public UpperAirFlagEnum U { get; set; }

        /// <summary>
        /// Auto
        /// </summary>
        public AutoFlagEnum A { get; set; }

        /// <summary>
        /// Office type
        /// </summary>
        public OfficeTypeFlagEnum C { get; set; }
    }
}
