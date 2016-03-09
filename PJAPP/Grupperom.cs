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
using System.Net;
using Newtonsoft.Json;
using Java.Lang;

namespace PJAPP
{
    [Activity(Label = "Grupperom")]
    public class Grupperom : Activity, BeaconManager.IServiceReadyCallback
    {
        BeaconManager _beaconManager;
        Region _region;
        ImageButton menuButton;
        ImageButton mainPage;

        private WebClient roomClient;
        private Uri servURL;

        public Handler mHandler = new Handler();

        public List<RomBeacon> romListeDB;
        public List<RomBeacon> romListe;
        public List<RomBeacon> romListeFinal;
        public ListView romListView;
        public gruppeRomListAdapter adapter;

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
            romListView = FindViewById<ListView>(Resource.Id.romListe1);
            roomClient = new WebClient();
            servURL = new Uri("http://pj3100.somee.com/GetRooms.php");

            romListe = new List<RomBeacon>();
            romListeFinal = new List<RomBeacon>();
            //rom45 58592 32436

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
                for (int i = 0; i < numberOfBeacons; i++)
                {

                    romListe.Add(new RomBeacon
                    {
                        BeaconUUID = e.Beacons[i].ProximityUUID.ToString(),
                        BeaconMajor = e.Beacons[i].Major,
                        BeaconMinor = e.Beacons[i].Minor,
                        distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi)
                    });
                }
            };

            roomClient.DownloadDataAsync(servURL);
            roomClient.DownloadDataCompleted += roomClient_DownloadDataCompleted;
        }

        private void roomClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                romListeDB = JsonConvert.DeserializeObject<List<RomBeacon>>(json);
            });
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
                return System.Math.Round(System.Math.Pow(ratio, 10), 2);
            }
            else
            {
                double accuracy = (0.89976) * System.Math.Pow(ratio, 7.7095) + 0.111;
                return System.Math.Round(accuracy, 2);
            }
        }
        protected override void OnResume()
        {
            base.OnResume();
            _beaconManager.Connect(this);
            
            TextView listeHeader = FindViewById<TextView>(Resource.Id.listeHeader1);
            _beaconManager.SetBackgroundScanPeriod(2000, 200);
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
               
                romListe = new List<RomBeacon>();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    romListe.Add(new RomBeacon
                    {
                        BeaconUUID = e.Beacons[i].ProximityUUID.ToString(),
                        BeaconMajor = e.Beacons[i].Major,
                        BeaconMinor = e.Beacons[i].Minor,
                        distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi)
                        //,
                    });
                }
                if (!(romListeDB.Count <= 0))
                {
                    foreach (RomBeacon b in romListeDB)
                    {
                        foreach (RomBeacon b2 in romListe)
                        {
                            if (romListeFinal.Count <= romListe.Count)
                            {
                                if (b.BeaconUUID == b2.BeaconUUID && b.BeaconMajor == b2.BeaconMajor && b.BeaconMinor == b2.BeaconMinor)
                                {
                                    b.distance = b2.distance;
                                }
                            }
                        }
                    }
                }

                if (!(romListeDB.Count <= 0))
                {
                    adapter = new gruppeRomListAdapter(this, romListeDB);
                    romListView.Adapter = adapter;
                    romListView.ItemClick += romListeClick;
                }
            };

            /*Runnable listUpdater = new Runnable(() =>
                    {
                        adapter.NotifyDataSetChanged();
                    });
                    mHandler.PostDelayed(listUpdater, 500);*/

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
            int minor = romListeDB[e.Position].BeaconMinor;
            int major = romListeDB[e.Position].BeaconMajor;
            string ID = romListeDB[e.Position].BeaconUUID;
            string name = romListeDB[e.Position].RomNavn;
            string prosjektor = romListeDB[e.Position].HarProsjektor;
            int plasser = romListeDB[e.Position].Plasser;

            var romDetaljIntent = new Intent(this, typeof(RomDetalj));
            romDetaljIntent.PutExtra("minor", minor);
            romDetaljIntent.PutExtra("major", major);
            romDetaljIntent.PutExtra("ID", ID);
            romDetaljIntent.PutExtra("name", name);
            romDetaljIntent.PutExtra("prosjektor", prosjektor);
            romDetaljIntent.PutExtra("plasser", plasser);
            StartActivity(romDetaljIntent);
            OnDestroy();
        }
    }
}