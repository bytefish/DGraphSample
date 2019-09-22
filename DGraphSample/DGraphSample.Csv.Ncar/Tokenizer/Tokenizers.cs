using TinyCsvParser.Tokenizer;
using static TinyCsvParser.Tokenizer.FixedLengthTokenizer;

namespace DGraphSample.Csv.Ncar.Tokenizer
{
    public static class Tokenizers
    {
        public static FixedLengthTokenizer StationTokenizer
        {
            get
            {
                var columns = new[]
                {
                    new ColumnDefinition(0, 2),
                    new ColumnDefinition(3, 19),
                    new ColumnDefinition(20, 24),
                    new ColumnDefinition(26, 29),
                    new ColumnDefinition(32, 37),
                    new ColumnDefinition(39, 45),
                    new ColumnDefinition(47, 54),
                    new ColumnDefinition(55, 60),
                    new ColumnDefinition(62, 63),
                    new ColumnDefinition(65, 66),
                    new ColumnDefinition(68, 69),
                    new ColumnDefinition(71, 72),
                    new ColumnDefinition(74, 75),
                    new ColumnDefinition(77, 78),
                };

                return new FixedLengthTokenizer(columns, true);
            }
        }
    }
}
