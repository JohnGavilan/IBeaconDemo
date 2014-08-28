using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using MonoTouch.CoreBluetooth;
using MonoTouch.CoreLocation;
using MonoTouch.CoreFoundation;
using MonoTouch.AVFoundation;
using MonoTouch.MultipeerConnectivity;

namespace Exhibitor.Mobile.iOS.Classes
{
    class iBeacon : Exhibitor.Mobile.IBBeaconIterface
    {
//        CBPeripheralManager peripheralMgr;
//        BTPeripheralDelegate peripheralDelegate;
        CLLocationManager locationMgr;
//        CLProximity previousProximity;

        List<CLBeaconRegion> Regions = new List<CLBeaconRegion>();

        public event IBBeaconDelegates.BeaconsInRangeHandler EnteredRange;
        public event IBBeaconDelegates.RegionEnteredHandler EnteredRegion;
        public event IBBeaconDelegates.RegionExitedHandler ExitedRegion;

        public void ServiceConnected()
        {
            locationMgr = new CLLocationManager();

            locationMgr.DidRangeBeacons += locationMgr_DidRangeBeacons;
            locationMgr.RegionEntered += locationMgr_RegionEntered;
            locationMgr.RegionLeft += locationMgr_RegionLeft;

        }

        void locationMgr_RegionLeft(object sender, CLRegionEventArgs e)
        {
            if (ExitedRegion != null)
            {
                //IBRegion r = null;
                if (e.Region != null)
                {
                    //r = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid, e.Region.UniqueId);
                }
                ExitedRegion(sender, new IBMonitorEventArgs());
            }
        }

        public string StartListening(List<IBBeacon> beacons)
        {


            foreach (IBBeacon b in beacons)
            {
                CLBeaconRegion r = new CLBeaconRegion(
					b.ProximityUuid == null ? new NSUuid("") : new NSUuid(b.ProximityUuid), 
					b.BeaconId);

				r.NotifyEntryStateOnDisplay = true;
				r.NotifyOnEntry = true;
				r.NotifyOnExit = true;

                locationMgr.StartMonitoring(r);
                locationMgr.StartRangingBeacons(r);
                Regions.Add(r);
            }


            return "Success";
        }

		public string ResumeListening()
		{
			foreach(CLBeaconRegion r in Regions)
			{
				locationMgr.StopMonitoring(r);
				locationMgr.StopRangingBeacons(r);
			}

			return "Success";
		}

        public string StopListening()
        {
            foreach(CLBeaconRegion r in Regions)
            {
                locationMgr.StopMonitoring(r);
                locationMgr.StopRangingBeacons(r);
            }

            return "Success";
        }

        void locationMgr_RegionEntered(object sender, CLRegionEventArgs e)
        {
            if (EnteredRegion != null)
            {
                IBRegion r = null;
                if (e.Region != null)
                {
                    //r = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid, e.Region.UniqueId);
                }
                EnteredRegion(sender, new IBMonitorEventArgs() );
            }
        }

        void locationMgr_DidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
        {
            if (EnteredRange != null)
            {
                if (e.Beacons.Length > 0)
                {

                    IBRangeEventArgs iba = new IBRangeEventArgs();
                    iba.Region = new IBRegion(e.Region.Major, e.Region.Minor, e.Region.ProximityUuid.ToString(), e.Region.Identifier);
                    iba.Beacons = new List<IBBeacon>();
                    foreach (CLBeacon b in e.Beacons)
                    {
                        int proximity = 0;
                        switch(b.Proximity)
                        {
                            case CLProximity.Immediate:
                                proximity = (int)IBProximityType.Immediate;
                                break;
                            case CLProximity.Near:
                                proximity = (int)IBProximityType.Near;
                                break;
                            case CLProximity.Far:
                                proximity = (int)IBProximityType.Far;
                                break;
                            case CLProximity.Unknown:
                                proximity = (int)IBProximityType.Unknown;
                                break;
                        }

                        iba.Beacons.Add(new IBBeacon()
                        {
                            Accuracy = b.Accuracy,
                            Major = (int)b.Major,
                            Minor = (int)b.Minor,
                            Proximity = proximity,
                            ProximityUuid = b.ProximityUuid.ToString(),
                            Rssi = b.Rssi
                        });
                    }

                    EnteredRange(sender, iba);
                }
            }

        }


//        class BTPeripheralDelegate : CBPeripheralManagerDelegate
//        {
//            public override void StateUpdated(CBPeripheralManager peripheral)
//            {
//                if (peripheral.State == CBPeripheralManagerState.PoweredOn)
//                {
//                    Console.WriteLine("powered on");
//                }
//            }
//        }
    }
}