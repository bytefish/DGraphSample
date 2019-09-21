// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using TinyCsvParser.TypeConverter;

namespace DGraphSample.Csv.Asos.Converter
{
    public class AsosTimeConverter : NullableConverter<TimeSpan?>
    {
        protected override bool InternalConvert(string value, out TimeSpan? result)
        {
            result = default(TimeSpan?);

            if(value == null)
            {
                return false;
            }

            var components = value.Split(".");

            if(components.Length != 2)
            {
                return false;
            }

            if(!int.TryParse(components[0], out int hours))
            {
                return false;
            }

            if(!int.TryParse(components[1], out int minutes))
            {
                return false;
            }

            result = new TimeSpan(hours, minutes, 0);

            return true;
        }
    }
}
