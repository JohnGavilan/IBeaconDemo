using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using RadiusNetworks.IBeaconAndroid;

namespace Exhibitor.Mobile.Droid
{
    [Activity(Label = "Exhibitor.Mobile", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity, IBeaconConsumer
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            Classes.iBeacon ib = new Classes.iBeacon();
            ib.Bind(this);

            Exhibitor.Mobile.ServiceLocator.Current.SetService<IBBeaconIterface>(ib);


            SetPage(App.GetMainPage());
        }

        public void OnIBeaconServiceConnect()
        {
            Exhibitor.Mobile.ServiceLocator.Current.GetService<IBBeaconIterface>().ServiceConnected();
        }
			
		protected override void OnResume ()
		{
			Exhibitor.Mobile.ServiceLocator.Current.GetService<IBBeaconIterface> ().StartListening ();
			base.OnResume ();
		}
		protected override void OnStop ()
		{
			Exhibitor.Mobile.ServiceLocator.Current.GetService<IBBeaconIterface> ().StopListening ();
			base.OnStop ();
		}
    }
}

