using System;
using TinyCsvParser.TypeConverter;

namespace DGraphSample.Csv.Asos.Converter
{
    public class MissingValuesConverter<TTargetType> : ITypeConverter<TTargetType>
    {
        private readonly string missingValueRepresentation;
        private readonly ITypeConverter<TTargetType> converter;

        public MissingValuesConverter(string missingValueRepresentation, ITypeConverter<TTargetType> converter)
        {
            this.missingValueRepresentation = missingValueRepresentation;
            this.converter = converter;
        }

        public bool TryConvert(string value, out TTargetType result)
        {
            if (string.Equals(missingValueRepresentation, value, StringComparison.Ordinal))
            {
                result = default(TTargetType);

                return true;
            }

            return converter.TryConvert(value, out result);
        }

        public Type TargetType
        {
            get { return typeof(float?); }
        }
    }
}
