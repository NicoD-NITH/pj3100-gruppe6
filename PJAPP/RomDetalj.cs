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
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace PJAPP
{
    [Activity(Label = "RomDetalj")]
    public class RomDetalj : Activity
    {
        ImageButton mainMenu;
        ImageButton menuButton;
        TextView romNavn;
        TextView plasser;
        TextView prosjektor;

        /*private WebClient roomClient;
        private Uri servURL;*/

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.romDetalj);

            int minor = Intent.GetIntExtra("minor" , 0);
            int major = Intent.GetIntExtra("major", 0);
            string name = Intent.GetStringExtra("name") ?? "Data not available";
            string ID = Intent.GetStringExtra("ID") ?? "Data not available";

            int storrelseText = Intent.GetIntExtra("plasser", 0);
            string prosjektorText = Intent.GetStringExtra("prosjektor") ?? "Data not available";

            romNavn = FindViewById<TextView>(Resource.Id.romNavn);
            plasser = FindViewById<TextView>(Resource.Id.plasser2);
            prosjektor = FindViewById<TextView>(Resource.Id.prosjektor2);
            romNavn.Text = name;
            plasser.Text = storrelseText.ToString();
            prosjektor.Text = prosjektorText.ToString();

            mainMenu = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);

            mainMenu.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

           
        }
    }
}