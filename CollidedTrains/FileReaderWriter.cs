using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CollidedTrains
{
    [Serializable]
    public class PathStations
    {
        public string Begin;
        public string End;
        public int Distance;

        public PathStations() { }

        public PathStations(string begin, string end, int distance) 
        {
            Begin = begin;
            End = end;
            Distance = distance;
        }
    }

    [Serializable]
    public class RouteTrain
    {
        public List<string> Route;

        public RouteTrain() { }

        public RouteTrain(params string[] route) { Route = route.ToList(); }
    }

    [Serializable]
    public class DataRailwaySystem
    {
        public List<string> Stations;

        public List<PathStations> Paths;
        
        public List<RouteTrain> Trains;

        public static DataRailwaySystem GetSampleSystem()
        {
            DataRailwaySystem dataRS = new DataRailwaySystem();

            dataRS.Stations = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };

            dataRS.Paths = new List<PathStations>();
            dataRS.Paths.Add(new PathStations("A", "B", 5));
            dataRS.Paths.Add(new PathStations("B", "C", 5));
            dataRS.Paths.Add(new PathStations("C", "D", 5));
            dataRS.Paths.Add(new PathStations("D", "E", 5));
            dataRS.Paths.Add(new PathStations("E", "F", 5));
            dataRS.Paths.Add(new PathStations("F", "G", 5));
            dataRS.Paths.Add(new PathStations("G", "H", 5));

            dataRS.Trains = new List<RouteTrain>();
            dataRS.Trains.Add(new RouteTrain("A", "B", "C", "D", "E", "F", "G", "H"));
            dataRS.Trains.Add(new RouteTrain("H", "G", "F"));
            dataRS.Trains.Add(new RouteTrain("F", "E", "D"));
            dataRS.Trains.Add(new RouteTrain("C", "D"));
            dataRS.Trains.Add(new RouteTrain("A", "B"));
            dataRS.Trains.Add(new RouteTrain("C", "B"));
            dataRS.Trains.Add(new RouteTrain("A", "B", "C"));
            dataRS.Trains.Add(new RouteTrain("E", "D", "C"));
            dataRS.Trains.Add(new RouteTrain("D", "C", "B"));
            dataRS.Trains.Add(new RouteTrain("D"));

            return dataRS;
        }
    }
    

    class FileReaderWriter
    {
        public static void SetDataRailwaySystem(string fileName)
        {
            var dataRS = DataRailwaySystem.GetSampleSystem();
            XmlSerializer formatter = new XmlSerializer(typeof(DataRailwaySystem));

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, dataRS);
            }
        }

        public static DataRailwaySystem GetDataRailwaySystem(string fileName)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(DataRailwaySystem));
            DataRailwaySystem dataRS;
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                dataRS = (DataRailwaySystem)formatter.Deserialize(fs);
                
            }
            return dataRS;
        }
    }
}
