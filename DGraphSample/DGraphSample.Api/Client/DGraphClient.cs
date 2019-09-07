// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using DGraphSample.Api.Generated;

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

        public Transaction NewReadOnlyTxn()
        {
            return NewTxn(true);
        }

        public Transaction NewTxn(bool isReadOnly = false)
        {
            return new Transaction(client, new TxnContext(), isReadOnly);
        }

        public async Task<Payload> AlterAsync(Operation operation, CancellationToken cancellationToken)
        {
            var payload = await client.AlterAsync(request: operation, cancellationToken: cancellationToken);

            return payload;
        }

        public async Task<Version> CheckVersionAsync(CancellationToken cancellationToken)
        {
            var check = new Check();

            return await client.CheckVersionAsync(check, cancellationToken: cancellationToken);
        }



    }
}