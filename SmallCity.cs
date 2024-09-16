using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Algo
{
    internal class SmallCity
    {
        public int cityId { set; get; }
        public int x { set; get; }
        public int y { set; get; }
        public int size { set; get; }
        public List<int> CityConnection { set; get; }
        public int distance { set; get; }

        public SmallCity(int cityId, int x, int y, int size, List<int> cityConnection, int distance)
        {
            this.cityId = cityId;
            this.x = x;
            this.y = y;
            this.size = size;
            CityConnection = cityConnection;
            this.distance = distance;
        }
    }
}
