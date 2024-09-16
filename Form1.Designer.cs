using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static System.Windows.Forms.LinkLabel;

namespace A_Star_Algo
{
    partial class Form1 : Form
    {

        List<City> CityList = new List<City>();
        List<City> smallCityList = new List<City>();
        List<City> CityListAll = new List<City>();
        List<Line> LineList = new List<Line>();
        City Start = new City();
        City End = new City();

        int Switch = 0;
        private System.ComponentModel.IContainer components = null;
        int cityCount = 0;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

            // Add MouseClick event handler
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);

        }

        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.White, 2);
            Pen greenPen = new Pen(Color.LightSeaGreen, 2);

            Brush brush = new SolidBrush(Color.White);
            Brush brush1 = new SolidBrush(Color.LightBlue);

            int width = (int)(this.ClientSize.Width - this.ClientSize.Width * 0.1);
            int height = (int)(this.ClientSize.Height - this.ClientSize.Height * 0.1);

            Random random = new Random();
            int CityCount = random.Next(10, 25);

            for (int c = 0; c < CityCount; c++)
            {
                int size = random.Next(30, 50);
                int x = random.Next((int)(size), (int)(width - size));
                int y = random.Next((int)(size), (int)(height - size));

                List<int> Connections = new List<int>();
                List<City> smallCityList = new List<City>();

                City city = new City(cityCount, x, y, size, Connections, smallCityList, 0);

                bool isOverlapping = false;

                foreach (City item in CityList)
                {
                    if ((x > item.x && x < item.x + size) || (y > item.y && y < item.y + size) ||
                (x + size > item.x && x + size < item.x + size) || (y + size > item.y && y + size < item.y + size))
                    {
                        isOverlapping = true;
                        break;
                    }
                }

                if (!isOverlapping)
                {
                    CityList.Add(city);
                    CityListAll.Add(city);
                    cityCount++;
                }
                else
                {
                    c--;
                }
            }

            // Generate small cities around each big city and connect them
            foreach (City bigCity in CityList)
            {
                int amount = random.Next(4, 15);

                for (int i = 0; i < amount; i++)
                {
                    int size = random.Next(15, 20);
                    int space = random.Next(size + 5, 30);

                    int x = random.Next(bigCity.x - bigCity.size - space, bigCity.x + bigCity.size + space);
                    int y = random.Next(bigCity.y - bigCity.size - space, bigCity.y + bigCity.size + space);

                    if ((x + size < bigCity.x || x > bigCity.x + bigCity.size) || (y + size < bigCity.y || y > bigCity.y + bigCity.size))
                    {
                        bool collisionDetected = false;

                        foreach (City existingSmallCity in bigCity.SmallCityList)
                        {
                            if (!((x + size < existingSmallCity.x || x > existingSmallCity.x + existingSmallCity.size) ||
                                  (y + size < existingSmallCity.y || y > existingSmallCity.y + existingSmallCity.size)))
                            {
                                collisionDetected = true;
                                break;
                            }
                        }

                        if (!collisionDetected)
                        {
                            List<int> emptyCityList = new List<int>();
                            List<City> emptyCityListList = new List<City>();
                            City smallCity = new City(cityCount, x, y, size, emptyCityList, emptyCityListList, 0);
                            bigCity.SmallCityList.Add(smallCity);
                            smallCityList.Add(smallCity);
                            cityCount++;
                            g.FillEllipse(brush, x, y, size, size);
                        }
                    }
                }

                // Connect small cities to each other within the big city
                foreach (City smallCity in bigCity.SmallCityList)
                {
                    HashSet<int> uniqueConnections = new HashSet<int>(); // Use HashSet to prevent duplicate connections

                    for (int i = 0; i < random.Next(0, bigCity.SmallCityList.Count); i++)
                    {
                        int connection = random.Next(0, bigCity.SmallCityList.Count);

                        if (connection != smallCity.cityId && !uniqueConnections.Contains(connection))
                        {
                            uniqueConnections.Add(connection);
                            List<int> list = smallCity.CityConnection;

                            City connectedCity = bigCity.SmallCityList.FirstOrDefault(c => c.cityId == connection);
                            if (connectedCity != null)
                            {
                                list.Add(connection);  // Add connection to the current small city
                                connectedCity.CityConnection.Add(smallCity.cityId);  // Add reverse connection

                                Line line = new Line(smallCity.x, smallCity.y, connectedCity.x, connectedCity.y, smallCity.cityId, connectedCity.cityId, (int)Math.Sqrt(Math.Pow(connectedCity.x - smallCity.x, 2) + Math.Pow(connectedCity.y - smallCity.y, 2)));
                                g.DrawLine(pen, new Point(smallCity.x + smallCity.size / 2, (smallCity.y + smallCity.size / 2)), new Point(connectedCity.x + connectedCity.size / 2, (connectedCity.y + connectedCity.size / 2)));
                                LineList.Add(line);
                            }
                        }
                    }

                    Line line2 = new Line(smallCity.x, smallCity.y, bigCity.x, bigCity.y, smallCity.cityId, bigCity.cityId, (int)Math.Sqrt(Math.Pow(bigCity.x - smallCity.x, 2) + Math.Pow(bigCity.y - smallCity.y, 2)));
                    g.DrawLine(pen, new Point(smallCity.x + smallCity.size / 2, (smallCity.y + smallCity.size / 2)), new Point(bigCity.x + bigCity.size / 2, (bigCity.y + bigCity.size / 2)));
                    LineList.Add(line2);

                    // Add connection from small city back to its big city
                    smallCity.CityConnection.Add(bigCity.cityId);
                    bigCity.CityConnection.Add(smallCity.cityId);  // Also add this small city to the big city's connections
                }
            }

            // Connect nearest small cities from different big cities
            foreach (City city1 in CityList)
            {
                foreach (City smallCity1 in city1.SmallCityList)
                {
                    City nearestSmallCity = null;
                    double minDistance = double.MaxValue;

                    // Find the nearest small city from a different big city
                    foreach (City city2 in CityList)
                    {
                        if (city1 != city2)
                        {
                            foreach (City smallCity2 in city2.SmallCityList)
                            {
                                double distance = Math.Sqrt(Math.Pow(smallCity1.x - smallCity2.x, 2) + Math.Pow(smallCity1.y - smallCity2.y, 2));
                                if (distance < minDistance)
                                {
                                    minDistance = distance;
                                    nearestSmallCity = smallCity2;
                                }
                            }
                        }
                    }

                    // Draw connection to the nearest small city
                    if (nearestSmallCity != null)
                    {
                        smallCity1.CityConnection.Add(nearestSmallCity.cityId);
                        nearestSmallCity.CityConnection.Add(smallCity1.cityId);  // Add reverse connection

                        g.DrawLine(greenPen, new Point(smallCity1.x + smallCity1.size / 2, smallCity1.y + smallCity1.size / 2), new Point(nearestSmallCity.x + nearestSmallCity.size / 2, nearestSmallCity.y + nearestSmallCity.size / 2));
                        Line line = new Line(smallCity1.x, smallCity1.y, nearestSmallCity.x, nearestSmallCity.y, smallCity1.cityId, nearestSmallCity.cityId, (int)Math.Sqrt(Math.Pow(nearestSmallCity.x - smallCity1.x, 2) + Math.Pow(nearestSmallCity.y - smallCity1.y, 2)));
                        LineList.Add(line);
                    }
                }
            }

            // Output cities and their connections
            foreach (City city in CityList)
            {
                Debug.WriteLine(city.ToString());
                g.FillEllipse(brush, city.x, city.y, city.size, city.size);
                g.FillEllipse(brush1, city.x + 5, city.y + 5, city.size - 10, city.size - 10);
            }
            CityListAll = CityList.Concat(smallCityList).ToList();
        }



        // MouseClick event handler to capture mouse position
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Point clickPosition = e.Location;


            using (Graphics g = this.CreateGraphics())
            {

                Pen pen = new Pen(Color.White, 2);
                Pen greenPen = new Pen(Color.LightSeaGreen, 2);

                Brush brush = new SolidBrush(Color.White);
                Brush brushDefault = new SolidBrush(Color.LightBlue);
                Brush brush1 = new SolidBrush(Color.LightSalmon);
                Brush brush2 = new SolidBrush(Color.Magenta);

                

               foreach (City city in CityList)
                {
                    if (!((clickPosition.X < city.x || clickPosition.X > city.x + city.size) || (clickPosition.Y < city.y || clickPosition.Y > city.y + city.size)))
                    {
                        if(Switch == 0)
                        {
                            g.FillEllipse(brush, city.x, city.y, city.size, city.size);
                            g.FillEllipse(brush1, city.x + 5, city.y + 5, city.size - 10, city.size - 10);

                            Start = city;
                        }
                        else if (Switch == 1)
                        {
                            g.FillEllipse(brush, city.x, city.y, city.size, city.size);
                            g.FillEllipse(brush2, city.x + 5, city.y + 5, city.size - 10, city.size - 10);

                            End = city;
                            setDistanceofCitys(End);
                            A_Star_Algorithm(g);

                        } else
                        {
                            Switch = 0;
                            foreach (City city1 in CityList)
                            {
                                g.FillEllipse(brush, city1.x, city1.y, city1.size, city1.size);
                                g.FillEllipse(brushDefault, city1.x + 5, city1.y + 5, city1.size - 10, city1.size - 10);
                            }
                            g.FillEllipse(brush, city.x, city.y, city.size, city.size);
                            g.FillEllipse(brush1, city.x + 5, city.y + 5, city.size - 10, city.size - 10);
                        }
                    }
                }

                Switch++;

            }
        }

        private void setDistanceofCitys(City city)
        {
            foreach (City city1 in CityListAll)
            {
                int Distance = (int)Math.Sqrt(Math.Pow(city.x - city1.x, 2) + Math.Pow(city.y - city1.y, 2));
                if(Distance > 0)
                {
                    city1.distance = Distance;
                    Debug.WriteLine(Distance);
                }
            }   
        }

        private void A_Star_Algorithm(Graphics g)
        {
            Brush brushDefault = new SolidBrush(Color.LightBlue);
            Brush brushPath = new SolidBrush(Color.Red);
            Pen pathPen = new Pen(Color.DarkRed, 2);

            Debug.WriteLine("Contents of LineList:");
            foreach (var line in LineList)
            {
                Debug.WriteLine(line.ToString());
            }

            City currentCity = Start;
            bool goalReached = false;
            HashSet<int> visitedCities = new HashSet<int>();
            Stack<City> pathStack = new Stack<City>();
            Dictionary<int, City> previousCities = new Dictionary<int, City>();

            Debug.WriteLine("Starting City: " + currentCity);

            g.FillEllipse(brushPath, currentCity.x, currentCity.y, currentCity.size, currentCity.size);
            pathStack.Push(currentCity);

            while (pathStack.Count > 0)
            {
                currentCity = pathStack.Peek(); 

                if (currentCity.cityId == End.cityId)
                {
                    goalReached = true;
                    Debug.WriteLine("Reached the destination city.");
                    break;
                }

                visitedCities.Add(currentCity.cityId);

                List<Line> connectedLines = LineList
                    .Where(line => line.connection1 == currentCity.cityId || line.connection2 == currentCity.cityId)
                    .ToList();


                List<Weight> potentialMoves = new List<Weight>();
                foreach (var line in connectedLines)
                {
                    int nextCityId = (line.connection1 == currentCity.cityId) ? line.connection2 : line.connection1;
                    if (visitedCities.Contains(nextCityId)) continue; 

                    City possibleNextCity = CityListAll.FirstOrDefault(c => c.cityId == nextCityId);
                    if (possibleNextCity == null)
                    {
                        Debug.WriteLine($"Connected city with id {nextCityId} not found.");
                        continue;
                    }

                    int gCost = line.distance; 
                    int hCost = GetHeuristic(possibleNextCity, End); 
                    int totalCost = gCost + hCost;

                    potentialMoves.Add(new Weight(totalCost, line));
                }

                if (potentialMoves.Count == 0)
                {
                    Debug.WriteLine("Dead end reached at city: " + currentCity + ". Backtracking...");
                    pathStack.Pop(); 
                    if (pathStack.Count > 0)
                    {
                        City previousCity = pathStack.Peek();
                        g.DrawLine(new Pen(Color.White, 2), new Point(currentCity.x + currentCity.size / 2, currentCity.y + currentCity.size / 2), new Point(previousCity.x + previousCity.size / 2, previousCity.y + previousCity.size / 2));
                        g.FillEllipse(brushDefault, currentCity.x, currentCity.y, currentCity.size, currentCity.size); 
                    }
                    continue; 
                }

                Weight bestMove = potentialMoves.OrderBy(w => w.weight).First();

                City nextCity1 = CityListAll.FirstOrDefault(c => c.cityId == bestMove.line.connection1);
                City nextCity2 = CityListAll.FirstOrDefault(c => c.cityId == bestMove.line.connection2);

                if (nextCity1 == null || nextCity2 == null)
                {
                    Debug.WriteLine("One or both cities not found in the list. Exiting.");
                    break;
                }

                City selectedNextCity = (currentCity.cityId == nextCity1.cityId) ? nextCity2 : nextCity1;

                if (visitedCities.Contains(selectedNextCity.cityId)) continue; 


                g.FillEllipse(brushPath, selectedNextCity.x, selectedNextCity.y, selectedNextCity.size, selectedNextCity.size);
                g.DrawLine(pathPen, new Point(currentCity.x + currentCity.size / 2, currentCity.y + currentCity.size / 2), new Point(selectedNextCity.x + selectedNextCity.size / 2, selectedNextCity.y + selectedNextCity.size / 2));

                Debug.WriteLine("Moving from City: " + currentCity + " to City: " + selectedNextCity);

                pathStack.Push(selectedNextCity); 
            }

            if (!goalReached)
            {
                Debug.WriteLine("Failed to reach the destination city.");
            }
        }

        // Heuristic function to estimate cost to the end city
        private int GetHeuristic(City current, City goal)
        {
            // Example heuristic: Euclidean distance
            int dx = Math.Abs(current.x - goal.x);
            int dy = Math.Abs(current.y - goal.y);
            return (int)Math.Sqrt(dx * dx + dy * dy);
        }


    }
}
