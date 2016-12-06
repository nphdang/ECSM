using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSM
{
    class Item
    {
        public Item()
        {
            this.id = -1;
            this.att = -1;
            this.val = -1;           
        }

        public int id { get; set; }
        public int att { get; set; } // att = {0, 1,..., n}
        public int val { get; set; }
    }
}
