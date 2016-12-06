using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ECSM
{
    class Node
    {
        public Node(ushort groups)
        {
            this.id = -1;
            this.att = -1;
            this.itemset = new List<Item>();
            this.Obidset = new List<int>[groups];
            for (ushort x = 0; x < groups; x++)
            {
                this.Obidset[x] = new List<int>();
            }
            this.pos = 0;
            this.total = 0;
            this.max_pro = 0;
            this.max_group = 0;
            this.dev = 0;
            this.chi = 0;
            this.p_value = 0;
            this.pEC = new List<int>();
            this.cEC = new List<int>();
            this.pL = new List<int>();
            this.cL = new List<int>();
        }

        public int id { get; set; }
        public long att { get; set; } // convert att to 2^att = {2^0, 2^1,..., 2^n}
        public List<Item> itemset { get; set; }
        public List<int>[] Obidset { get; set; }
        public int pos { get; set; } // max sup(X,G_i) on D
        public int total { get; set; } // support on D
        public double max_pro { get; set; }
        public int max_group { get; set; } // dominant group
        public double dev { get; set; } // max support difference
        public double chi { get; set; }
        public double p_value { get; set; }
        public List<int> pEC { get; set; } // parent EC
        public List<int> cEC { get; set; } // child EC
        public List<int> pL { get; set; } // parent L
        public List<int> cL { get; set; } // child L
    }
}
