using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollidedTrains
{
    class Train
    {
        private List<int> station;

        private List<int> time;
        
        public Train()
        {
            station = new List<int>();
            time = new List<int>();
        }

        public int Count { get { return station.Count; } }
        
        public int GetStation(int id)
        {
            if (id >= 0 && station.Count > 0)
            {
                return station[id];
            }
            else return -1;
        }

        public int GetTime(int id)
        {
            if (id >= 0 && time.Count > 0)
            {
                return time[id];
            }
            else return -1;
        }

        public void Add(int station, int time)
        {
            this.station.Add(station);
            this.time.Add(time);
        }

        public bool TryGetCollisionOnStation(Train train, out int station, out int time)
        {
            station = -1;
            time = -1;
            for (int x = 0; x < this.Count; x++)
            {
                int xStation = this.GetStation(x);
                int xTime = this.GetTime(x);
                for (int y = 0; y < train.Count; y++)
                {
                    int yStation = train.GetStation(y);
                    int yTime = train.GetTime(y);
                    if (xStation == yStation && xTime == yTime)
                    {
                        station = xStation;
                        time = xTime;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGetCollisionBetweenStations(Train train, out int station1, out int station2)
        {
            station1 = -1;
            station2 = -1;
            for (int x = 0; x < this.Count - 1; x++)
            {
                int xStation1 = this.GetStation(x);
                int xStation2 = this.GetStation(x + 1);

                for (int y = 0; y < train.Count - 1; y++)
                {
                    int yStation1 = train.GetStation(y);
                    int yStation2 = train.GetStation(y + 1);
                    if (xStation1 == yStation2 && xStation2 == yStation1)
                    {
                        int xTime1 = this.GetTime(x);
                        int xTime2 = this.GetTime(x + 1);
                        int yTime1 = train.GetTime(y);
                        int yTime2 = train.GetTime(y + 1);
                        if (xTime2 > yTime1 && xTime1 < yTime2)
                        {
                            station1 = xStation1;
                            station2 = xStation2;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
