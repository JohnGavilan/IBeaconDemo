using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exhibitor.Mobile
{
    public interface IBBeaconIterface
    {
        string StartListening(List<IBBeacon> Beacons);
        string StopListening();
        void ServiceConnected();

        event IBBeaconDelegates.RegionEnteredHandler EnteredRegion;
        event IBBeaconDelegates.RegionExitedHandler ExitedRegion;
        event IBBeaconDelegates.BeaconsInRangeHandler EnteredRange;
    }

    public class IBBeaconDelegates
    {
        public delegate void RegionEnteredHandler(Object sender, IBMonitorEventArgs e);
        public delegate void RegionExitedHandler(Object sender, IBMonitorEventArgs e);
        public delegate void BeaconsInRangeHandler(Object sender, IBRangeEventArgs e);

    }

    public class IBMonitorEventArgs : EventArgs
    {
        public IBRegion Region { get; set; }
        public int State { get; set; }
    }

    public class IBRangeEventArgs : EventArgs
    {
        public IBRegion Region { get; set; }
        public ICollection<IBBeacon> Beacons { get; set; } 
    }

    public class IBBeacon
    {
        public double Accuracy { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Proximity { get; set; }
        public string ProximityUuid { get; set; }
        public string BeaconId { get; set; }
        public int Rssi { get; set; }

		public string UniqueID
		{
			get{
				return ProximityUuid + ":" + Major.ToString () + ":" + Minor.ToString () + ":" + BeaconId;
			}
		}
    }

    public class IBRegion
    {
        public IBRegion(
             object major,
             object minor,
             string proximityUuid,
             string uniqueId)
        {
			if (major != null) Major = Convert.ToInt32(major);
			if (minor != null) Minor = Convert.ToInt32(minor);
            if (uniqueId != null) UniqueId = uniqueId;
            if (proximityUuid != null) ProximityUuid = proximityUuid;
        }

        public int Major { get; set; }
        public int Minor { get; set; }
        public string UniqueId { get; set; }
        public string ProximityUuid { get; set; }
    }

    public enum IBProximityType
    {
        Immediate = 0,
        Near = 1,
        Far = 2,
        Unknown = 4
    }

}
