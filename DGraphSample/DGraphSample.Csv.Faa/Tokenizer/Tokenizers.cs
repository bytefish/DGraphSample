// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using TinyCsvParser.Tokenizer;

namespace DGraphSample.Csv.Faa.Tokenizer
{
    public static class Tokenizers
    {
        public static ITokenizer FaaAircraftTokenizer
        {
            get
            {
                return new QuotedStringTokenizer(',');
            }
        }
    }
}
