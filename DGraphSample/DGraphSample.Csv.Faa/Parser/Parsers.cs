// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Faa.Mapper;
using DGraphSample.Csv.Faa.Model;
using TinyCsvParser;

namespace DGraphSample.Csv.Faa.Parser
{
    public static class Parsers
    {
        public static CsvParser<FaaAircraft> MetarStationParser
        {
            get
            {
                var tokenizer = Tokenizer.Tokenizers.FaaAircraftTokenizer;

                var options = new CsvParserOptions(skipHeader: true,  tokenizer: tokenizer);

                return new CsvParser<FaaAircraft>(options, new FaaAircraftMapper());
            }
        }
    }
}