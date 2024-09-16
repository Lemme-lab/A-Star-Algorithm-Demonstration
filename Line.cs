using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Algo
{
    internal class Line
    {

        public override string ToString()
        {
            return $"Line: ({x1}, {y1}) to ({x2}, {y2}), Connections: [{connection1}, {connection2}], Distance: {distance}";
        }

        public Line(int x1, int y1, int x2, int y2, int connection1, int connection2, int distance)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            this.connection1 = connection1;
            this.connection2 = connection2;
            this.distance = distance;
        }

        public int x1 { set; get; }
        public int y1 { set; get; }
        public int x2 { set; get; }
        public int y2 { set; get; }
        public int connection1 { set; get; }
        public int connection2 { set; get; }
        public int distance { set; get; }
    }
}
