using System;
using System.Collections.Generic;
using System.Text;

namespace DGraphSample.Dto
{
    public class WeatherDataDto
    {
        /// <summary>
        /// Three or four character site identifier
        /// </summary>
        public string station { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public float? lon { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public float? lat { get; set; }

        /// <summary>
        /// Timestamp of the observation
        /// </summary>
        public DateTime? valid { get; set; }

        /// <summary>
        /// Air Temperature in Fahrenheit, typically @ 2 meters
        /// </summary>
        public float? tmpf { get; set; }

        /// <summary>
        /// Air Temperature in Celsius, typically @ 2 meters
        /// </summary>
        public float? tmpc { get; set; }

        /// <summary>
        /// Dew Point Temperature in Fahrenheit, typically @ 2 meters
        /// </summary>
        public float? dwpf { get; set; }

        /// <summary>
        /// Dew Point Temperature in Celsius, typically @ 2 meters
        /// </summary>
        public float? dwpc { get; set; }

        /// <summary>
        /// Relative Humidity in %
        /// </summary>
        public float? relh { get; set; }

        /// <summary>
        /// Wind Direction in degrees from north
        /// </summary>
        public float? drct { get; set; }

        /// <summary>
        /// Wind Speed in knots
        /// </summary>
        public float? sknt { get; set; }

        /// <summary>
        /// One hour precipitation for the period from the observation time to the time 
        /// of the previous hourly precipitation reset. This varies slightly by site. Values 
        /// are in inches. This value may or may not contain frozen precipitation melted by 
        /// some device on the sensor or estimated by some other means. 
        /// 
        /// Unfortunately, we do not know of an authoritative database denoting which station 
        /// has which sensor.
        /// </summary>
        public float? p01i { get; set; }

        /// <summary>
        /// Pressure altimeter in inches
        /// </summary>
        public float? alti { get; set; }

        /// <summary>
        /// Sea Level Pressure in millibar
        /// </summary>
        public float? mslp { get; set; }

        /// <summary>
        /// Visibility in miles
        /// </summary>
        public float? vsby_mi { get; set; }

        /// <summary>
        /// Visibility in miles
        /// </summary>
        public float? vsby_km { get; set; }

        /// <summary>
        /// Wind Gust in knots
        /// </summary>
        public float? gust { get; set; }

        /// <summary>
        /// Sky Level 1 Coverage
        /// </summary>
        public string skyc1 { get; set; }

        /// <summary>
        /// Sky Level 2 Coverage
        /// </summary>
        public string skyc2 { get; set; }

        /// <summary>
        /// Sky Level 3 Coverage
        /// </summary>
        public string skyc3 { get; set; }

        /// <summary>
        /// Sky Level 4 Coverage
        /// </summary>
        public string skyc4 { get; set; }

        /// <summary>
        /// Sky Level 1 Altitude in feet
        /// </summary>
        public float? skyl1 { get; set; }

        /// <summary>
        /// Sky Level 2 Altitude in feet
        /// </summary>
        public float? skyl2 { get; set; }

        /// <summary>
        /// Sky Level 3 Altitude in feet
        /// </summary>
        public float? skyl3 { get; set; }

        /// <summary>
        /// Sky Level 4 Altitude in feet
        /// </summary>
        public float? skyl4 { get; set; }

        /// <summary>
        /// Present Weather Codes (space seperated)
        /// </summary>
        public string wxcodes { get; set; }

        /// <summary>
        /// Apparent Temperature (Wind Chill or Heat Index) in Fahrenheit
        /// </summary>
        public float? feelf { get; set; }

        /// <summary>
        /// Apparent Temperature (Wind Chill or Heat Index) in Celsius
        /// </summary>
        public float? feelc { get; set; }

        /// <summary>
        /// Ice Accretion over 1 Hour (inches)
        /// </summary>
        public float? ice_accretion_1hr { get; set; }

        /// <summary>
        /// Ice Accretion over 3 Hours (inches)
        /// </summary>
        public float? ice_accretion_3hr { get; set; }

        /// <summary>
        /// Ice Accretion over 6 Hours (inches)
        /// </summary>
        public float? ice_accretion_6hr { get; set; }

        /// <summary>
        /// Peak Wind Gust (from PK WND METAR remark) (knots)
        /// </summary>
        public float? peak_wind_gust { get; set; }

        /// <summary>
        /// Peak Wind Gust Direction (from PK WND METAR remark) (deg)
        /// </summary>
        public float? peak_wind_drct { get; set; }

        /// <summary>
        /// Peak Wind Gust Time hour of the day (from PK WND METAR remark) 
        /// </summary>
        public int? peak_wind_time_hh { get; set; }

        /// <summary>
        /// Peak Wind Gust Time minute of the day (from PK WND METAR remark) 
        /// </summary>
        public int? peak_wind_time_MM { get; set; }

        /// <summary>
        /// unprocessed reported observation in METAR format
        /// </summary>
        public string metar { get; set; }
    }
}
