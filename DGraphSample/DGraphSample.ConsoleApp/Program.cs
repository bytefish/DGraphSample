using System;
using System.Reflection.Emit;
using System.Threading;
using DGraphSample.Api;
using DGraphSample.Api.Client;
using Grpc.Core;

namespace DGraphSample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DGraphClient("127.0.0.1", 9080, ChannelCredentials.Insecure);

            var operation = new Operation
            {
                Schema = "",
            };

            client.AlterAsync(operation, CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
