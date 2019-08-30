// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Google.Protobuf;
using System;
using System.Globalization;
using System.Text;

namespace DGraphSample.Api.Utils
{
    public static class TypeConverters
    {
        public static ByteString Convert(DateTime dateTime)
        {
            var dateTimeString = dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
            var dateTimeBytes = Encoding.UTF8.GetBytes(dateTimeString);

            return ByteString.CopyFrom(dateTimeBytes);
        }
    }
}
