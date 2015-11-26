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

        //TEST VARIABLER
        public List<string> mNames;
        public ListView mListView;
        //TEST VARIABLER STOP

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.gruppeRom);
            // Create your application here
            //SetContentView(Resource.Layout.Main);
            _beaconManager = new BeaconManager(this);

            _region = new Region("SomeBeaconIdentifier", "b9407f30-f5f8-466e-aff9-25556b57fe6d");

            //Button button = FindViewById<Button>(Resource.Id.MyButton);

           /* _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (sender, e) =>
            {
                // Do something as the device has entered in region for the Estimote.
                int numberOfBeacons = e.Beacons.Count;
                /*if (numberOfBeacons == 1)
				{
					button.Text = string.Format("Found beacon: '{0}'", e.Beacons[0].MacAddress);
				}
				else
				{
					button.Text = string.Format("Found {0} beacons.", e.Beacons.Count);
				}

                mListView = FindViewById<ListView>(Resource.Id.romListe1);
                mNames = new List<string>();

                //BeaconRoomsClass[] myBeacons = new BeaconRoomsClass[e.Beacons.Count];

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    //myBeacons[i] = new BeaconRoomsClass("Room " + i, e.Beacons[i].Major, e.Beacons[i].Minor, calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi));
                    mNames.Add(e.Beacons[i].Major + " " + e.Beacons[i].Minor + " Distance: " + calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi));
                }
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mNames);
                mListView.Adapter = adapter;
                //ArrayAdapter<BeaconRoomsClass> adapter = new ArrayAdapter<BeaconRoomsClass>(this, Android.Resource.Layout.SimpleListItem1, 4);
                //mListView.Adapter = adapter;
            };
            _beaconManager.ExitedRegion += (sender, e) =>
            {
                // Do something as the device has left the region for the Estimote.
                //button.Text = "Beacon(s) disappeared.";
            };
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
            /*_beaconManager = new BeaconManager(this);

            _region = new Region("SomeBeaconIdentifier", "b9407f30-f5f8-466e-aff9-25556b57fe6d");*/

            //Button button = FindViewById<Button>(Resource.Id.MyButton);

            _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (sender, e) =>
            {
                // Do something as the device has entered in region for the Estimote.
                int numberOfBeacons = e.Beacons.Count;
                /*if (numberOfBeacons == 1)
				{
					button.Text = string.Format("Found beacon: '{0}'", e.Beacons[0].MacAddress);
				}
				else
				{
					button.Text = string.Format("Found {0} beacons.", e.Beacons.Count);
				}*/

                mListView = FindViewById<ListView>(Resource.Id.romListe1);
                mNames = new List<string>();

                //BeaconRoomsClass[] myBeacons = new BeaconRoomsClass[e.Beacons.Count];

               
                    for (int i = 0; i < numberOfBeacons; i++)
                    {
                        //myBeacons[i] = new BeaconRoomsClass("Room " + i, e.Beacons[i].Major, e.Beacons[i].Minor, calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi));
                        mNames.Add(e.Beacons[i].Major + " " + e.Beacons[i].Minor + " Distance: " + calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi));
                    }
               
                
                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, mNames);
                    mListView.Adapter = adapter;
                
                
                //ArrayAdapter<BeaconRoomsClass> adapter = new ArrayAdapter<BeaconRoomsClass>(this, Android.Resource.Layout.SimpleListItem1, 4);
                //mListView.Adapter = adapter;
            };
            _beaconManager.ExitedRegion += (sender, e) =>
            {
                // Do something as the device has left the region for the Estimote.
                //button.Text = "Beacon(s) disappeared.";
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


