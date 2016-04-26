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

        WebClient veilederClient;
        Uri servURL;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.veileder);

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
    }
}