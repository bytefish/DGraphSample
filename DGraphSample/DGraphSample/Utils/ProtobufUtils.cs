// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Google.Protobuf;
using Newtonsoft.Json;

namespace DGraphSample.Utils
{
    public static class ProtobufUtils
    {
        public static TResult Deserialize<TResult>(ByteString json)
            where TResult : class
        {
            if (json == null)
            {
                return null;
            }

            var jsonString = json.ToStringUtf8();

            return JsonConvert.DeserializeObject<TResult>(jsonString);
        }
    }
}
