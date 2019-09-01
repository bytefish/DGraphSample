// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using DGraphSample.Api;
using DGraphSample.Api.Utils;

namespace DGraphSample.DGraph.Utils
{
    public class NQuadBuilder
    {
        private readonly List<NQuad> nquads;
        private readonly string subject;

        public NQuadBuilder(string subject)
        {
            this.subject = subject;
            this.nquads = new List<NQuad>();
        }

        public NQuadBuilder Add(string predicate, string objectValue)
        {
            nquads.Add(new NQuad
            {
                Subject = subject, Predicate = predicate, ObjectValue = new Value
                {
                    StrVal = objectValue
                }
            });

            return this;
        }

        public NQuadBuilder Add(string predicate, float objectValue)
        {
            
            nquads.Add(new NQuad
            {
                Subject = subject,
                Predicate = predicate,
                ObjectValue = new Value
                {
                    DoubleVal = objectValue
                }
            });

            return this;
        }

        public NQuadBuilder Add(string predicate, DateTime objectValue)
        {
            
            nquads.Add(new NQuad
            {
                Subject = subject,
                Predicate = predicate,
                ObjectValue = new Value
                {
                    DatetimeVal = TypeConverters.Convert(objectValue)
                }
            });

            return this;
        }

        public NQuadBuilder Add(string predicate, ulong uid)
        {

            nquads.Add(new NQuad
            {
                Subject = subject,
                Predicate = predicate,
                ObjectValue = new Value
                {
                    UidVal = uid
                }
            });

            return this;
        }


        public List<NQuad> Build()
        {
            return nquads;
        }
    }
}
