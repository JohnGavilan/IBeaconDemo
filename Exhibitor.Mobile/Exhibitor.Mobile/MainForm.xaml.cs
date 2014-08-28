using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Exhibitor.Mobile
{
    public partial class MainForm
    {

        public IBBeaconIterface ibeacon;
		IBBeacon currentbeacon;

        public MainForm()
        {
            InitializeComponent();
            btnStartListening.Clicked += btnStartListening_Clicked;
			btnStopListening.Clicked += btnStopListening_Clicked;
            ibeacon = ServiceLocator.Current.GetService<IBBeaconIterface>();
            ibeacon.EnteredRegion += ibeacon_EnteredRegion;
            ibeacon.ExitedRegion += ibeacon_ExitedRegion;
            ibeacon.EnteredRange += ibeacon_EnteredRange;
			currentbeacon = new IBBeacon() {Proximity = 4};

        }



        void ibeacon_ExitedRegion(object sender, IBMonitorEventArgs e)
        {
            //throw new NotImplementedException();
			currentbeacon = new IBBeacon() {Proximity = 4};
        }

        void ibeacon_EnteredRange(object sender, IBRangeEventArgs e)
		{
			bool changed = false;
			foreach (IBBeacon beacon in e.Beacons) {
				//update the proximity of the currentbeacon
				if (currentbeacon.UniqueID == beacon.UniqueID) {
					//changed = currentbeacon.Proximity != beacon.Proximity;
					currentbeacon.Proximity = beacon.Proximity;
				} else {
					if (beacon.Proximity < currentbeacon.Proximity || beacon.Proximity == 0) {
						currentbeacon = beacon;
						changed = true;
					}
				}
			}


			#region Inline Data
//			string str = "<b>" + currentbeacon.ProximityUuid +
//				"</b> <br/> Proximity: " + currentbeacon.Proximity +
//				"<br/> Major: " + currentbeacon.Major +
//				"<br/> Minor: " + currentbeacon.Minor +
//				"<br/> ID: " + currentbeacon.BeaconId +
//				"<br/> Accuracy: " + currentbeacon.Accuracy;
//
//
//			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
//				if (changed) {
//					HtmlWebViewSource wvc = new HtmlWebViewSource ();
//					wvc.Html = str;
//					MainWebView.Source = wvc;
//				}
//
//				StatusLabel.Text = str;
//			});
			#endregion

			#region Sample Data
			string str = "";

			if(currentbeacon.ProximityUuid != null)
			{
				SampleData.Exhibit exhibit = SampleData.Exhibits.Find(o => 
					currentbeacon.ProximityUuid.ToUpper().EndsWith(o.BeaconUUID.ToUpper()) &&
					o.BeaconMajor == currentbeacon.Major &&
					o.BeaconMinor == currentbeacon.Minor
				);


				if(exhibit == null)
				{
					str = "No Beacons In Range";
					Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
						StatusLabel.Text = str;
					});
				}
				else
				{
					str = exhibit.Name + " - " + exhibit.Description;
					Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
						if (changed) {
							UrlWebViewSource wvs = new UrlWebViewSource();
							wvs.Url = exhibit.ExternalURL;
							MainWebView.Source = wvs;
						}
						StatusLabel.Text = str;
					});
				}
			}
			#endregion

		}

        void ibeacon_EnteredRegion(object sender, IBMonitorEventArgs e)
        {
			currentbeacon = new IBBeacon() {Proximity = 4};
        }

        protected override void OnAppearing()
        {
			MainWebView.VerticalOptions = LayoutOptions.FillAndExpand;
			MainWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
        }


        void btnStartListening_Clicked(object sender, EventArgs e)
        {
            List<IBBeacon> beacons = new List<IBBeacon>();

			#region Inline Data
//			beacons.Add(new IBBeacon() { ProximityUuid = "8DEEFBB9-F738-4297-8040-96668BB44281", BeaconId = "Beacon1", Major = 1, Minor = 5129 });
//			beacons.Add(new IBBeacon() { ProximityUuid = "8DEEFBB9-F738-4297-8040-96668BB44281", BeaconId = "Beacon2", Major = 1, Minor = 5098 });
//          beacons.Add(new IBBeacon() { ProximityUuid = "2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6", BeaconId = "USB Beacon" });
			#endregion

			#region Sample Data
			foreach (SampleData.Exhibit exhibit in SampleData.Exhibits) {
				beacons.Add (new IBBeacon () {
					ProximityUuid = exhibit.BeaconUUID,
					BeaconId = exhibit.Name,
					Major = exhibit.BeaconMajor,
					Minor = exhibit.BeaconMinor
				});
			}
			#endregion

            ibeacon.StartListening(beacons);
        }

		void btnStopListening_Clicked (object sender, EventArgs e)
		{
			ibeacon.StopListening ();
		}

    }
}
