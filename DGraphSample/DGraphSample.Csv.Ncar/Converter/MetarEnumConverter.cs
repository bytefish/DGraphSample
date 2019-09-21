using System;
using TinyCsvParser.TypeConverter;

namespace DGraphSample.Csv.Ncar.Converter
{
    public class MetarEnumConverter<TTargetType> : ITypeConverter<TTargetType>
        where TTargetType : struct
    {
        private readonly ITypeConverter<TTargetType> converter;

        public MetarEnumConverter()
        {
            this.converter = new EnumConverter<TTargetType>();
        }

        public bool TryConvert(string value, out TTargetType result)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                value = "None";
            }

            return converter.TryConvert(value, out result);
        }

        public Type TargetType
        {
            get { return typeof(float?); }
        }
    }
}
