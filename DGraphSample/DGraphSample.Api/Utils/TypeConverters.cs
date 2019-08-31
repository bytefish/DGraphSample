// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Google.Protobuf;
using System;
using System.Globalization;
using System.Text;

namespace DGraphSample.Api.Utils
{
    public static class TypeConverters
    {
        private const string Rfc3339DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.fffK";

        public static ByteString Convert(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return null;
            }

            return Convert(dateTime.Value);
        }

        public static ByteString Convert(DateTime dateTime)
        {
            var dateTimeString = ToRFC3390(dateTime);
            var dateTimeBytes = Encoding.UTF8.GetBytes(dateTimeString);

            return ByteString.CopyFrom(dateTimeBytes);
        }

        private static string ToRFC3390(DateTime dateTime)
        {
            return dateTime.ToString(Rfc3339DateTimeFormat, DateTimeFormatInfo.InvariantInfo);
        }
    }
}
