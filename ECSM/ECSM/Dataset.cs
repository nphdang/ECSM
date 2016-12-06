using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECSM
{
    class Dataset
    {
        public Dataset(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.cls = 0;
            this.row_cls = null;
            this.data = new int[row][];
            for (int i = 0; i < this.row; i++)
            {
                this.data[i] = new int[this.col];
            }
        }

        public int row { get; set; }
        public int col { get; set; }
        public ushort cls { get; set; }
        public int[] row_cls { get; set; }
        public int[][] data { get; set; }
    }
}
