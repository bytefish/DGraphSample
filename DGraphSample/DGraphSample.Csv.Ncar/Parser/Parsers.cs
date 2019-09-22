// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DGraphSample.Csv.Ncar.Mapper;
using DGraphSample.Csv.Ncar.Model;
using TinyCsvParser;

namespace DGraphSample.Csv.Ncar.Parser
{
    public static class Parsers
    {
        public static CsvParser<MetarStation> MetarStationParser
        {
            get
            {
                var tokenizer = Tokenizer.Tokenizers.StationTokenizer;

                // The CSV file has ! as Comments, that we want to ignore while parsing 
                // the data, because it will yield invalid data:
                var options = new CsvParserOptions(
                    skipHeader: false, 
                    commentCharacter: "!", 
                    tokenizer: tokenizer);

                return new CsvParser<MetarStation>(options, new MetarStationMapper());
            }
        }
    }
}