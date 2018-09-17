using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace CollidedTrains
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string fileName = "test.xml";
            DataRailwaySystem dataRS;
            try
            {
                // Here is a simple example of a file with system settings, 
                // but the method can overwrite an existing file
                // FileReaderWriter.SetDataRailwaySystem(fileName);
                dataRS = FileReaderWriter.GetDataRailwaySystem(fileName);
                RailwaySystem god = new RailwaySystem(dataRS);
                god.CheckCollided();
            }
            catch(Exception e)
            {
                Console.WriteLine("Не удалось корректно считать заданный файл.");
                Console.WriteLine(e.ToString());                
            }
            Console.ReadLine();
        }
    }

    
    class RailwaySystem
    {
        private List<string> nameStations;

        private List<PathStations> paths = new List<PathStations>();

        private List<Train> trains = new List<Train>();

        public RailwaySystem(DataRailwaySystem dataRS) 
        {
            nameStations = dataRS.Stations;
            paths = dataRS.Paths;
            
            for (int i = 0; i < dataRS.Trains.Count; i++)
            {
                int time = 0, id1, id2 = nameStations.IndexOf(dataRS.Trains[i].Route[0]);
                trains.Add(new Train());
                trains[i].Add(id2, time);
                for (int j = 1; j < dataRS.Trains[i].Route.Count; j++)
                {
                    id1 = id2;
                    id2 = nameStations.IndexOf(dataRS.Trains[i].Route[j]);
                    time += paths.Find(x =>
                        ((x.Begin == nameStations[id1]) && (x.End == nameStations[id2])) ||
                        ((x.End == nameStations[id1]) && (x.Begin == nameStations[id2]))
                    ).Distance;
                    trains[i].Add(id2, time);
                }
            }
                       
        }

        public void CheckCollided()
        {
            int[][] trainStation;
            bool[][] trainEdge;
            int countTrains = trains.Count;

            trainStation = new int[nameStations.Count][];
            for (int i = 0; i < nameStations.Count; i++)
                trainStation[i] = new int[countTrains];

            trainEdge = new bool[paths.Count][];
            for (int i = 0; i < paths.Count; i++)
                trainEdge[i] = new bool[countTrains];

            checkCollisionOnStation(trainStation);
            checkCollisionBetweenStations(trainEdge);
            print(trainStation, trainEdge);

        }

        private void print(int[][] trainStation, bool[][] trainEdge)
        {
            Console.WriteLine();
            Console.WriteLine("Поезда, сталкивающиеся на станциях 'станция: номер поезда(время)':");
            int n = trainStation.Length;
            bool wasNot = true, flag;
            for (int i = 0; i < n; i++)
            {
                string s = nameStations[i] + ": ";
                flag = false;
                for (int j = 0; j < trainStation[i].Length; j++)
                {
                    if(trainStation[i][j] > 0){
                        s += j + "(" + trainStation[i][j] + "); ";
                        flag = true;
                    }
                }
                if (flag)
                {
                    Console.WriteLine(s);
                    wasNot = false;
                }
            }
            if (wasNot) Console.WriteLine("Таких поездов не было.");
            wasNot = true;
            Console.WriteLine();
            Console.WriteLine("Поезда, сталкивающиеся на путях 'станция-станция: поезд':");
            n = trainEdge.Length;
            for (int i = 0; i < n; i++)
            {
                string s = paths[i].Begin + "-" + paths[i].End + ": ";
                flag = false;
                for (int j = 0; j < trainEdge[i].Length; j++)
                {
                    if (trainEdge[i][j])
                    {
                        s += j + " ";
                        flag = true; 
                    }
                }
                if (flag)
                {
                    Console.WriteLine(s);
                    wasNot = false;
                }
            }
            if (wasNot) Console.WriteLine("Таких поездов не было.");
        }

        private void checkCollisionOnStation(int[][] trainStation)
        {
            int station, time;
            for (int i = 0; i < trains.Count; i++)
            {
                for (int k = 0; k < trains.Count; k++)
                {
                    if (k != i)
                    {
                        if (trains[i].TryGetCollisionOnStation(trains[k], out station, out time))
                        {
                            trainStation[station][i] = time;
                            trainStation[station][k] = time;
                        }
                    }
                }
            }

        }

        private void checkCollisionBetweenStations(bool[][] trainEdge)
        {
            int station1, station2;
            for (int i = 0; i < trains.Count; i++)
            {

                for (int k = 0; k < trains.Count; k++)
                {
                    if (k != i)
                    {

                        if (trains[i].TryGetCollisionBetweenStations(trains[k], out station1, out station2))
                        {
                            int id = paths.IndexOf(
                                paths.First(x =>
                                    (x.Begin == nameStations[station1] && x.End == nameStations[station2]) ||
                                    (x.Begin == nameStations[station2] && x.End == nameStations[station1])
                                )
                            );
                            trainEdge[id][i] = true;
                            trainEdge[id][k] = true;
                        }
                    }
                }
            }
        }
    }

}
