using System;
using System.Collections.Generic;

namespace PCME.MOPDB
{
    public partial class TrainStation
    {
        public TrainStation()
        {
            TrainStationBelong = new HashSet<TrainStationBelong>();
        }

        public string StationId { get; set; }
        public string StationName { get; set; }

        public ICollection<TrainStationBelong> TrainStationBelong { get; set; }
    }
}
