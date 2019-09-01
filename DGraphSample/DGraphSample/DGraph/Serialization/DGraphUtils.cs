using Google.Protobuf;
using Newtonsoft.Json;

namespace DGraphSample.DGraph.Serialization
{
    public static class DGraphUtils
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
