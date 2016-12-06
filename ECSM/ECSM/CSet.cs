using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSM
{
    class CSet
    {
        public CSet(ushort groups)
        {
            this.g_id = -1;
            this.C_itemset = new List<Item>();
            this.Sup = 0;
            this.Pro = new double[groups];
            this.Dev = 0;
            this.Chi = 0;
        }

        public int g_id { get; set; }
        public double Sup { get; set; }
        public List<Item> C_itemset { get; set; }
        public double[] Pro { get; set; }
        public double Dev { get; set; } // support difference
        public double Chi { get; set; }
    }
}
