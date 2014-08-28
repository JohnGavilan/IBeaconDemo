using System;
using System.Collections.Generic;
using System.Linq;

namespace Exhibitor.Mobile
{
	public class SampleData
	{
		static List<Exhibit> _Exhibits = null;

		public static List<Exhibit> Exhibits
		{
			get{
				if (_Exhibits == null) {
					LoadExhibits ();
				}

				return _Exhibits;
			}
		}

		private static void LoadExhibits()
		{
			_Exhibits = new List<Exhibit> ();
			_Exhibits.Add (new SampleData.Exhibit (){ 
				Name = "Porsche Carrera GT", 
				Description = "Awesome Car", 
				ExternalURL = "https://www.youtube.com/watch?v=vE_WqdKbTvY",
				BeaconUUID = "8DEEFBB9-F738-4297-8040-96668BB44281",
				BeaconMajor = 1,
				BeaconMinor = 5129
			} );

			_Exhibits.Add (new SampleData.Exhibit (){ 
				Name = "Ferrari FF", 
				Description = "Kick Ass Car", 
				ExternalURL = "https://www.youtube.com/watch?v=NiXQ00ZRV38",
				BeaconUUID = "8DEEFBB9-F738-4297-8040-96668BB44281",
				BeaconMajor = 1,
				BeaconMinor = 5098
			} );

			_Exhibits.Add (new SampleData.Exhibit (){ 
				Name = "Lamborghini Aventador", 
				Description = "Batman would drive this Car", 
				ExternalURL = "https://www.youtube.com/watch?v=UZiHPLGh-Ek",
				BeaconUUID = "2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6",
				BeaconMajor = 1,
				BeaconMinor = 1
			} );
		}

		public class Exhibit
		{
			public string Name = "";
			public string Description = "";
			public string ExternalURL = "";
			public string BeaconUUID = "";
			public int BeaconMajor = 0;
			public int BeaconMinor = 0;
		}
	}
}

