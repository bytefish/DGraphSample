using System;
using System.Collections.Generic;
using System.Text;

namespace DGraphSample.DGraph.Model
{
    public class City
    {
        public ulong UID { get; set; }

        public string Name { get; set; }

        public ulong State { get; set; }

        public ulong Country { get; set; }
    }
}
