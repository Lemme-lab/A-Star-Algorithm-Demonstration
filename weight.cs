using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Algo
{
    internal class Weight
    {
        public Weight(int weight, Line line)
        {
            this.weight = weight;
            this.line = line;
        }

        public int weight { set; get; }
        public Line line { set; get; }
    }
}
