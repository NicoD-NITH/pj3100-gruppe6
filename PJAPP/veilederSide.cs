using System;
using System.Collections.Generic;
using System.Text;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using Newtonsoft.Json;

namespace PJAPP
{
    [Activity(Label = "veilederSide")]
    
    public class veilederSide : Activity
    {
        ImageButton menuButton;
        ImageButton mainPage;

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

            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            veilederView = FindViewById<ListView>(Resource.Id.veilederView);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };

            veilederClient = new WebClient();
            servURL = new Uri("http://pj3100.somee.com/GetVeileder.php");

            veilederClient.DownloadDataAsync(servURL);
            veilederClient.DownloadDataCompleted += VeilederClient_DownloadDataCompleted;
            
        }

        private void VeilederClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                string json = Encoding.UTF8.GetString(e.Result);
                veilederList = JsonConvert.DeserializeObject<List<veileder>>(json);
                adapter = new veilederListAdapter(this, veilederList);
                veilederView.Adapter = adapter;
            });
        }
    }
}