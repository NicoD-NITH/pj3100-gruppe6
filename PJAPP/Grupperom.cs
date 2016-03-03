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
using EstimoteSdk;

namespace PJAPP
{
    [Activity(Label = "Grupperom")]
    public class Grupperom : Activity, BeaconManager.IServiceReadyCallback
    {
        BeaconManager _beaconManager;
        Region _region;
        ImageButton menuButton;
        ImageButton mainPage;

        public List<RomBeacon> romListe;
        public ListView romListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.gruppeRom);

            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };

            _beaconManager = new BeaconManager(this);
            _region = new Region("SomeBeaconIdentifier", "b9407f30-f5f8-466e-aff9-25556b57fe6d");

            TextView listeHeader = FindViewById<TextView>(Resource.Id.listeHeader1);
           
        }

        

        protected static double calculateDistance(int txPower, double rssi)
        {
            if (rssi == 0)
            {
                return -1.0;
            }

            double ratio = rssi * 1.0 / txPower;
            if (ratio < 1.0)
            {
                return Math.Round(Math.Pow(ratio, 10), 2);
            }
            else
            {
                double accuracy = (0.89976) * Math.Pow(ratio, 7.7095) + 0.111;
                return Math.Round(accuracy, 2);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            _beaconManager.Connect(this);

            TextView listeHeader = FindViewById<TextView>(Resource.Id.listeHeader1);

            _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (sender, e) =>
            {
                int numberOfBeacons = e.Beacons.Count;
                if (numberOfBeacons == 0)
                {
                    listeHeader.Text = string.Format("Fant ingen rom i nærheten");
                }
                else
                {
                    listeHeader.Text = string.Format("Fant følgende {0} rom.", numberOfBeacons);
                }
                romListView = FindViewById<ListView>(Resource.Id.romListe1);
                romListe = new List<RomBeacon>();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    //beacon 1 min: 29070, maj: 17476, UUID: b9407f30-f5f8-466e-aff9-25556b57fe6d
                    //beacon 2: min: 7607, maj: 29066, UUID: b9407f30-f5f8-466e-aff9-25556b57fe6d
                    //beacon 3: min: 985, maj: 43313, UUID: b9407f30-f5f8-466e-aff9-25556b57fe6d
                    //beacon 4: min: 36063, maj: 37641, UUID: b9407f30-f5f8-466e-aff9-25556b57fe6d
                    /*romListe.Add(newRomBeacon
                      {
                        minor = e.Beacons[i].Minor,
                        major = e.Beacons[i].Major,
                        ID = E.Beacons.[i].ProximityUUID
                      });
                      */
                    romListe.Add(new RomBeacon
                    {
                        minor = e.Beacons[i].Minor,
                        major = e.Beacons[i].Major,
                        //distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi),
                        UUID = e.Beacons[i].ProximityUUID
                    });
                }
                foreach(RomBeacon b in romListe)
                {
                    if(b.minor == 29070 && b.major == 17476)
                    {
                        b.name = "Rom 39";
                    } else if (b.minor == 7607 && b.major == 29066)
                    {
                        b.name = "Rom 38";
                    } else if (b.minor == 985 && b.major == 43313)
                    {
                        b.name = "Rom 40";
                    } else if (b.minor == 36063 && b.major == 37641)
                    {
                        b.name = "Rom 41";
                    }
                }

                gruppeRomListAdapter adapter = new gruppeRomListAdapter(this, romListe);
                //ArrayAdapter<RomBeacon> adapter = new ArrayAdapter<RomBeacon>(this, Android.Resource.Layout., mNames);
                romListView.Adapter = adapter;
                romListView.ItemClick += romListeClick;
            };
        }
        public void OnServiceReady()
        {
            _beaconManager.StartMonitoring(_region);
        }
        protected override void OnDestroy()
        {
            _beaconManager.Disconnect();
            base.OnDestroy();
        }
        private void romListeClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int minor = romListe[e.Position].minor;
            int major = romListe[e.Position].major;
            string ID = romListe[e.Position].UUID.ToString();
            string name = romListe[e.Position].name;

            var romDetaljIntent = new Intent(this, typeof(RomDetalj));
            romDetaljIntent.PutExtra("minor", minor);
            romDetaljIntent.PutExtra("major", major);
            romDetaljIntent.PutExtra("ID", ID);
            romDetaljIntent.PutExtra("name", name);
            StartActivity(romDetaljIntent);
        }

    }
}