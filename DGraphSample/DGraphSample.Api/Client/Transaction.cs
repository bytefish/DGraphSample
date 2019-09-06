// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;

namespace DGraphSample.Api.Client
{
    public class Transaction
    {
        private readonly TxnContext context;
        private readonly Dgraph.DgraphClient client;

        private readonly bool readOnly = false;

        private bool finished = false;
        private bool mutated = false;

        public Transaction(Dgraph.DgraphClient client, TxnContext context, bool readOnly)
        {
            this.client = client;
            this.context = context;
            this.readOnly = readOnly;
        }

        public Task<Response> QueryAsync(string query, CancellationToken cancellationToken)
        {
            return QueryAsync(query, null, cancellationToken);
        }

        public async Task<Response> QueryAsync(string query, Dictionary<string, string> vars, CancellationToken cancellationToken)
        {
            if (finished)
            {
                throw new Exception("A finished Transaction cannot be queried");
            }

            Request request = new Request();

            request.Query = query;

            if (vars != null)
            {
                request.Vars.Add(vars);
            }

            request.StartTs = context.StartTs;
            request.ReadOnly = readOnly;

            Response response = await client.QueryAsync(request: request);

            MergeContext(response.Txn);

            return response;
        }

        public async Task<Assigned> MutateAsync(Mutation mutation, CancellationToken cancellationToken)
        {
            if (readOnly)
            {
                throw new Exception("A Read-Only Transaction cannot be mutated");
            }

            if (finished)
            {
                throw new Exception("A finished Transaction cannot be mutated");
            }

            mutated = true;

            mutation.StartTs = context.StartTs;

            Assigned assigned;

            try
            {
                assigned = await client.MutateAsync(request: mutation, cancellationToken: cancellationToken);

                if (mutation.CommitNow)
                {
                    finished = true;
                }

                MergeContext(assigned.Context);
            }
            catch (RpcException e)
            {
                try
                {
                    await DiscardAsync(cancellationToken);
                }
                catch
                {
                    // TODO This shouldn't go unnoticed.
                }

                throw e;
            }

            return assigned;
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            if (readOnly)
            {
                throw new Exception("A Read-Only Transaction cannot be commited");
            }

            if (finished)
            {
                throw new Exception("A Finished Exception cannot be commited");
            }

            finished = true;

            if (mutated)
            {
                await client.CommitOrAbortAsync(request: context, cancellationToken: cancellationToken);
            }
        }

        private void MergeContext(TxnContext source)
        {
            if (source != null)
            {
                if (context.StartTs == 0)
                {
                    context.StartTs = source.StartTs;
                }

                if (context.StartTs != source.StartTs)
                {
                    throw new Exception("A StartTs mismatch occured");
                }

                context.Keys.AddRange(source.Keys);
                context.Preds.AddRange(source.Preds);
            }
        }

        private async Task DiscardAsync(CancellationToken cancellationToken)
        {
            if (!finished)
            {
                finished = true;

                if (mutated)
                {
                    await client.CommitOrAbortAsync(request: context, cancellationToken: cancellationToken);
                }
            }
        }
    }
}