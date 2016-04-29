using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using EstimoteSdk;
using System.Net;
using Newtonsoft.Json;
using Android.Support.V4.Widget;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace PJAPP
{
    [Activity(Label = "Grupperom")]
    public class Grupperom : Activity, BeaconManager.IServiceReadyCallback
    {
        BeaconManager _beaconManager;
        Region _region;
        ImageButton menuButton;
        ImageButton mainPage;
        SwipeRefreshLayout swiperefresh;
        TextView infoText;
        Button hjelpButton;
        private WebClient roomClient;
        private Uri servURL;

        public Handler mHandler = new Handler();

        public List<RomBeacon> romListeDB;
        public List<RomBeacon> romListe;
        public ListView romListView;
        public gruppeRomListAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.gruppeRom);

            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            infoText = FindViewById<TextView>(Resource.Id.ingenInfo);
            hjelpButton = FindViewById<Button>(Resource.Id.HjelpButton);

            hjelpButton.Visibility = ViewStates.Gone;
            infoText.Visibility = ViewStates.Gone;

            swiperefresh = FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            swiperefresh.Refresh += Swiperefresh_Refresh;

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            _beaconManager = new BeaconManager(this);
            _region = new Region("SomeBeaconIdentifier", "b9407f30-f5f8-466e-aff9-25556b57fe6d");

            romListView = FindViewById<ListView>(Resource.Id.romListe1);
            roomClient = new WebClient();
            servURL = new Uri("http://pj3100.somee.com/GetRooms.php");

            romListe = new List<RomBeacon>();

            romListeDB = new List<RomBeacon>();

            _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (sender, e) =>
            {
                int numberOfBeacons = e.Beacons.Count;

                    for (int i = 0; i < numberOfBeacons; i++)
                    {
                        romListe.Add(new RomBeacon
                        {
                            BeaconUUID = e.Beacons[i].ProximityUUID.ToString(),
                            BeaconMajor = e.Beacons[i].Major,
                            BeaconMinor = e.Beacons[i].Minor,
                            distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi).ToString()
                        });
                    }
            };

            roomClient.DownloadDataAsync(servURL);
            roomClient.DownloadDataCompleted += roomClient_DownloadDataCompleted;

            if (romListeDB.Count == 0)
            {
                infoText.Visibility = ViewStates.Visible;
                infoText.Text = "Ingenting her, swipe ned for å fornye.";
            }
            else
            {
                infoText.Visibility = ViewStates.Gone;
            }
        }

        private void Swiperefresh_Refresh(object sender, EventArgs e)
        {
            /*BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.RunWorkerAsync();*/
            if(!roomClient.IsBusy)
            {
                swiperefresh.Refreshing = true;
                romListeDB.Clear();
                if (!roomClient.IsBusy)
                {
                    roomClient.DownloadDataAsync(servURL);
                    roomClient.DownloadDataCompleted += roomClient_DownloadDataCompleted;
                }

                _beaconManager.Connect(this);

                _beaconManager.SetBackgroundScanPeriod(2000, 0);
                _beaconManager.EnteredRegion += (senderB, Be) =>
                {
                    int numberOfBeacons = Be.Beacons.Count;

                    romListe.Clear();

                        for (int i = 0; i < numberOfBeacons; i++)
                        {
                            romListe.Add(new RomBeacon
                            {
                                BeaconUUID = Be.Beacons[i].ProximityUUID.ToString(),
                                BeaconMajor = Be.Beacons[i].Major,
                                BeaconMinor = Be.Beacons[i].Minor,
                                distance = calculateDistance(Be.Beacons[i].MeasuredPower, Be.Beacons[i].Rssi).ToString()
                            });
                    }
                    
                    if (!(romListeDB.Count <= 0))
                    {
                        foreach (RomBeacon b in romListeDB)
                        {
                            foreach (RomBeacon b2 in romListe)
                            {
                                if (b.BeaconUUID == b2.BeaconUUID && b.BeaconMajor == b2.BeaconMajor && b.BeaconMinor == b2.BeaconMinor)
                                {
                                    b.distance = b2.distance.ToString() + "m";
                                    if (b.distance.Equals(""))
                                    {
                                        b.distance = "Ingen kontakt";
                                    }
                                }
                            }
                        }
                    }
                };
                adapter.NotifyDataSetChanged();
                swiperefresh.Refreshing = false;
            }
            
        }

        /*private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                swiperefresh.Refreshing = false;
            });
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            swiperefresh.Refreshing = true;
            romListeDB.Clear();
            if(!roomClient.IsBusy)
            {
                roomClient.DownloadDataAsync(servURL);
                roomClient.DownloadDataCompleted += roomClient_DownloadDataCompleted;
            }

            _beaconManager.Connect(this);

            _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (senderB, Be) =>
            {
                int numberOfBeacons = Be.Beacons.Count;

                romListe.Clear();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    romListe.Add(new RomBeacon
                    {
                        BeaconUUID = Be.Beacons[i].ProximityUUID.ToString(),
                        BeaconMajor = Be.Beacons[i].Major,
                        BeaconMinor = Be.Beacons[i].Minor,
                        distance = calculateDistance(Be.Beacons[i].MeasuredPower, Be.Beacons[i].Rssi)
                    });
                }
                if (!(romListeDB.Count <= 0))
                {
                    foreach (RomBeacon b in romListeDB)
                    {
                        foreach (RomBeacon b2 in romListe)
                        {
                            if (b.BeaconUUID == b2.BeaconUUID && b.BeaconMajor == b2.BeaconMajor && b.BeaconMinor == b2.BeaconMinor)
                            {
                                b.distance = b2.distance;
                            }
                        }
                    }
                }
            };
            adapter.NotifyDataSetChanged();
            
        }*/

        private void roomClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                romListeDB = JsonConvert.DeserializeObject<List<RomBeacon>>(json);
                infoText.Visibility = ViewStates.Gone;
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

        string parseDistance(double calculatedDistance)
        {
            if(calculatedDistance < 10)
            {
                return "<10m";
            }
            else if(calculatedDistance > 8 && calculatedDistance  < 20)
            {
                return "<20m";
            } else
            {
                return ">20m";
            }
            
        }
        protected override void OnResume()
        {
            base.OnResume();
            _beaconManager.Connect(this);
            _beaconManager.SetBackgroundScanPeriod(2000, 0);
            _beaconManager.EnteredRegion += (sender, e) =>
            {
                int numberOfBeacons = e.Beacons.Count;
               
                romListe = new List<RomBeacon>();

                for (int i = 0; i < numberOfBeacons; i++)
                {
                    romListe.Add(new RomBeacon
                    {
                        BeaconUUID = e.Beacons[i].ProximityUUID.ToString(),
                        BeaconMajor = e.Beacons[i].Major,
                        BeaconMinor = e.Beacons[i].Minor,
                        distance = calculateDistance(e.Beacons[i].MeasuredPower, e.Beacons[i].Rssi).ToString()
                    });
                }
                
                if (!(romListeDB.Count <= 0) && !(romListe.Count <= 0))
                {
                    foreach (RomBeacon b in romListeDB)
                    {
                        foreach (RomBeacon b2 in romListe)
                        {
                            if (b.BeaconUUID == b2.BeaconUUID && b.BeaconMajor == b2.BeaconMajor && b.BeaconMinor == b2.BeaconMinor && (b2.distance != "0"))
                            {
                                b.distance = b2.distance.ToString() + "m";
                            }else
                            {
                                b.distance = "Ingen kontakt";
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
            int isBookable = romListeDB[e.Position].IsBookable;
            string bookingStamp = romListeDB[e.Position].bookingStamp;

            var romDetaljIntent = new Intent(this, typeof(RomDetalj));
            romDetaljIntent.PutExtra("minor", minor);
            romDetaljIntent.PutExtra("major", major);
            romDetaljIntent.PutExtra("ID", ID);
            romDetaljIntent.PutExtra("name", name);
            romDetaljIntent.PutExtra("prosjektor", prosjektor);
            romDetaljIntent.PutExtra("plasser", plasser);
            romDetaljIntent.PutExtra("isBookable", isBookable);
            romDetaljIntent.PutExtra("bookingStamp", bookingStamp);
            StartActivity(romDetaljIntent);
            OnDestroy();
        }
    }
}