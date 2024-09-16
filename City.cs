using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Algo
{
    internal class City
    {
        public int cityId { set; get; }
        public int x { set; get; }
        public int y { set; get; } 
        public int size { set; get; }
        public List<int> CityConnection { set; get; }
        public List<City> SmallCityList = new List<City>();
        public int distance { set; get; }

        public City()
        {
        }

        public override string ToString()
        {
            string cityConnections = string.Join(", ", CityConnection);
            string smallCities = string.Join(", ", SmallCityList.Select(c => c.cityId));

            return $"City ID: {cityId}, X: {x}, Y: {y}, Size: {size}, Distance: {distance}, " +
                   $"Connections: [{cityConnections}], Small Cities: [{smallCities}]";
        }

        public City(int cityId, int x, int y, int size, List<int> cityConnection, List<City> smallCityList, int distance)
        {
            this.cityId = cityId;
            this.x = x;
            this.y = y;
            this.size = size;
            CityConnection = cityConnection;
            SmallCityList = smallCityList;
            this.distance = distance;
        }
    }
}
