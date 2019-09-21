// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Ncar.Mapper;
using DGraphSample.Csv.Ncar.Tokenizers;
using DGraphSample.Csv.Ncar.Model;
using TinyCsvParser;

namespace DGraphSample.Csv.Parser
{
    public static class Parsers
    {
        public static CsvParser<MetarStation> MetarStationParser
        {
            get
            {
                var tokenizer = Tokenizers.StationTokenizer;
                var options = new CsvParserOptions(true, "!", tokenizer);

                return new CsvParser<MetarStation>(options, new MetarStationMapper());
            }
        }
    }
}