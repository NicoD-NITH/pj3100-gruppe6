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

        public List<RomBeacon> mNames;
        public ListView mListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.gruppeRom);

            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            _beaconManager = new BeaconManager(this);
            _region = new Region("SomeBeaconIdentifier", "b9407f30-f5f8-466e-aff9-25556b57fe6d");

            TextView listeHeader = FindViewById<TextView>(Resource.Id.listeHeader1);
            /*
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

                mListView = FindViewById<ListView>(Resource.Id.romListe1);
                mNames = new List<string>();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    mNames.Add(e.Beacons[i].Major + " " + e.Beacons[i].Minor + " Distance: " + calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi));
                }
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mNames);
                mListView.Adapter = adapter;
            */
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
                mListView = FindViewById<ListView>(Resource.Id.romListe1);
                mNames = new List<RomBeacon>();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    mNames.Add(new RomBeacon { minor = e.Beacons[i].Minor, major = e.Beacons[i].Major, distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi), name = "rom" + i});
                }
                gruppeRomListAdapter adapter = new gruppeRomListAdapter(this, mNames);
                //ArrayAdapter<RomBeacon> adapter = new ArrayAdapter<RomBeacon>(this, Android.Resource.Layout., mNames);
                mListView.Adapter = adapter;
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
    }
}