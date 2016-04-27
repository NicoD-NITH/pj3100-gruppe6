using System;
using System.Collections.Generic;
using System.Text;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using Newtonsoft.Json;
using Android.Content;
using Android.Support.V4.Widget;
using System.IO;
using Android.Preferences;

namespace PJAPP
{
    [Activity(Label = "veilederSide")]
    
    public class veilederSide : Activity
    {
        ImageButton menuButton;
        ImageButton mainPage;
        Button hjelpButton;
        //SwipeRefreshLayout swiperefresh;

        public veilederListAdapter adapter;
        public ListView veilederView;
        public List<veileder> veilederList;

        string student;

        WebClient veilederClient;
        Uri servURL;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.veileder);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            student = prefs.GetString("userName", "");

            /*swiperefresh = FindViewById<SwipeRefreshLayout>(Resource.Id.swiperefresh);
            swiperefresh.Refresh += Swiperefresh_Refresh;*/

            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            veilederView = FindViewById<ListView>(Resource.Id.veilederView);
            hjelpButton = FindViewById<Button>(Resource.Id.HjelpButton);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            if(canGetHelp())
            {
                hjelpButton.Visibility = ViewStates.Visible;
            } else
            {
                hjelpButton.Visibility = ViewStates.Invisible;
            }
            hjelpButton.Click += delegate
            {
                StartActivity(typeof(HelpList));
            };
            veilederClient = new WebClient();
            servURL = new Uri("http://pj3100.somee.com/GetVeileder.php");

            veilederClient.DownloadDataAsync(servURL);
            veilederClient.DownloadDataCompleted += VeilederClient_DownloadDataCompleted;
            
        }

        /*private void Swiperefresh_Refresh(object sender, EventArgs e)
        {
            if(!veilederClient.IsBusy)
            {
                veilederClient.DownloadDataAsync(servURL);
                veilederClient.DownloadDataCompleted += VeilederClient_DownloadDataCompleted;
            }
        }*/

        private void VeilederClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                veilederList = JsonConvert.DeserializeObject<List<veileder>>(json);
                adapter = new veilederListAdapter(this, veilederList);
                veilederView.Adapter = adapter;
                veilederView.ItemClick += VeilederView_ItemClick;

            });
            
        }

        private void VeilederView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            string name = veilederList[e.Position].VeilederNavn;
            string FagID = veilederList[e.Position].FagID;
            string LoginName = veilederList[e.Position].LoginName;

            var veilederHelpIntent = new Intent(this, typeof(VeilederHelp));
            veilederHelpIntent.PutExtra("name", name);
            veilederHelpIntent.PutExtra("FagID", FagID);
            veilederHelpIntent.PutExtra("LoginName", LoginName);

            StartActivity(veilederHelpIntent);
        }

        public class canData
        {
            public string navn { get; set; }
        }

        public bool canGetHelp()
        {
            try
            {
                canData DataObj = new canData();
                DataObj.navn = student;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);

                string url = "http://pj3100.somee.com/canGetHelp.php";

                HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(url);

                newRequest.Method = "POST";

                string postData = JSONString;

                byte[] pdata = Encoding.UTF8.GetBytes(postData);

                newRequest.ContentType = "application/x-www-form-urlencoded";
                newRequest.ContentLength = pdata.Length;

                Stream myStream = newRequest.GetRequestStream();
                myStream.Write(pdata, 0, pdata.Length);

                WebResponse myResponse = newRequest.GetResponse();

                Stream responseStream = myResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(responseStream);

                string result = streamReader.ReadToEnd();


                streamReader.Close();
                responseStream.Close();
                myStream.Close();

                if (result.Equals("1"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException ex)
            {
                string _exception = ex.ToString();
                Toast error = Toast.MakeText(this, _exception, ToastLength.Long);
                error.Show();
                Console.WriteLine("--->" + _exception);
                return false;
            }
        }
    }
}