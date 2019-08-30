// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace DGraphSample.Api.Client
{
    public class DGraphClient
    {
        private readonly Dgraph.DgraphClient client;

        public DGraphClient(string host, int port, ChannelCredentials credentials)
        {
            Channel channel = new Channel(host, port, credentials);

            this.client = new Dgraph.DgraphClient(new DefaultCallInvoker(channel));
        }

        public Transaction NewTxn()
        {
            return new Transaction(client, new TxnContext(), false);
        }

        public Transaction NewReadOnlyTxn()
        {
            return new Transaction(client, new TxnContext(), true);
        }

        public async Task<Payload> AlterAsync(Operation operation, CancellationToken cancellationToken)
        {
            var payload = await client.AlterAsync(request: operation, cancellationToken: cancellationToken);

            return payload;
        }
    }
}