using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RadiusNetworks.IBeaconAndroid;

namespace Exhibitor.Mobile.Droid.Classes
{
    class iBeacon : Exhibitor.Mobile.IBBeaconIterface
    {
        public event IBBeaconDelegates.RegionEnteredHandler EnteredRegion;
        public event IBBeaconDelegates.RegionExitedHandler ExitedRegion;
        public event IBBeaconDelegates.BeaconsInRangeHandler EnteredRange;

        IBeaconManager beaconMgr;
        List<NotifiersStruct> Notifiers = new List<NotifiersStruct>();

        public MonitorNotifier monitorNotifier;
        public RangeNotifier rangeNotifier;

        struct NotifiersStruct
        { 
            public Region monitoringRegion;
            public Region rangingRegion;
        }

        public string StartListening(List<IBBeacon> beacons)
        {
            if (beaconMgr.CheckAvailability())
            {

                foreach(IBBeacon b in beacons)
                {
                    NotifiersStruct nr = new NotifiersStruct();

                    nr.monitoringRegion = new Region(
                        b.BeaconId,
                        b.ProximityUuid, 
                        b.Major == 0 ? null : (Java.Lang.Integer)b.Major,
                        b.Minor == 0 ? null : (Java.Lang.Integer)b.Minor);
                    
                    nr.rangingRegion = new Region(
                        b.BeaconId, 
                        b.ProximityUuid,
                        b.Major == 0 ? null : (Java.Lang.Integer)b.Major,
                        b.Minor == 0 ? null : (Java.Lang.Integer)b.Minor);

                    beaconMgr.StartMonitoringBeaconsInRegion(nr.monitoringRegion);
                    beaconMgr.StartRangingBeaconsInRegion(nr.rangingRegion);

                    Notifiers.Add(nr);
                }
                return "Success";
            }
            else
            {
                return "IBeacon Service is not Connected";
            }
            
        }

        public string StopListening()
        {
            foreach (NotifiersStruct nr in Notifiers)
            { 
                beaconMgr.StopMonitoringBeaconsInRegion(nr.monitoringRegion);
                beaconMgr.StopMonitoringBeaconsInRegion(nr.rangingRegion);
            }
            return "Success";
        }

		public string ResumeListening()
		{
			foreach (NotifiersStruct nr in Notifiers)
			{ 
				beaconMgr.StartMonitoringBeaconsInRegion(nr.monitoringRegion);
				beaconMgr.StartMonitoringBeaconsInRegion(nr.rangingRegion);
			}
			return "Success";
		}

        public void Bind(IBeaconConsumer ibeaconconsumer)
        {
            beaconMgr = IBeaconManager.GetInstanceForApplication((Context)ibeaconconsumer);
            beaconMgr.Bind(ibeaconconsumer);

            monitorNotifier = new MonitorNotifier();
            monitorNotifier.EnterRegionComplete += EnteredRegionHandler;
            monitorNotifier.ExitRegionComplete += ExitedRegionHandler;

            rangeNotifier = new RangeNotifier();
            rangeNotifier.DidRangeBeaconsInRegionComplete += EnteredRangeHandler;

		}

		public void ServiceConnected ()
		{
            beaconMgr.SetMonitorNotifier(monitorNotifier);
            beaconMgr.SetRangeNotifier(rangeNotifier);
		}

        void EnteredRegionHandler(object sender, MonitorEventArgs e)
        {
            if (EnteredRegion != null) 
            {
                IBRegion r = null;
                if(e.Region != null)
                { 
                    r = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid, e.Region.UniqueId);
                }
                EnteredRegion(sender, new IBMonitorEventArgs() { Region = r, State = e.State });
            }
        }

        void ExitedRegionHandler(object sender, MonitorEventArgs e)
        {
            if (ExitedRegion != null)
            {
                IBRegion r = null;
                if (e.Region != null)
                {
                    r = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid, e.Region.UniqueId);
                }
                ExitedRegion(sender, new IBMonitorEventArgs() { Region = r, State = e.State });
            }
        }

        void EnteredRangeHandler(object sender, RangeEventArgs e)
        {
            if (EnteredRange != null)
            {
                IBRangeEventArgs iba = new IBRangeEventArgs();
                iba.Region = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid, e.Region.UniqueId);
                iba.Beacons = new List<IBBeacon>();
                foreach (IBeacon b in e.Beacons)
                {
                    iba.Beacons.Add(new IBBeacon()
                    {
                        Accuracy = b.Accuracy,
                        Major = b.Major,
                        Minor = b.Minor,
                        Proximity = b.Proximity,
                        ProximityUuid = b.ProximityUuid,
                        Rssi = b.Rssi
                    });
                }

                EnteredRange(sender, iba);
            }
        }

    }
}