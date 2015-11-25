using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace PJAPP
{
    [Activity(Label = "PJAPP", MainLauncher = true, Icon = "@drawable/icon")]

    public class MainActivity : Activity
    {
        private Button loggInnButton;

        protected override void OnCreate(Bundle bundle)
        {
            
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            loggInnButton = FindViewById<Button>(Resource.Id.loggInnButton);

            loggInnButton.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
           
        }
    }
}

