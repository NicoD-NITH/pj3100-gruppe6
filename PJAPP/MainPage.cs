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

namespace PJAPP
{
    [Activity(Label = "MainPage")]
    public class MainPage : Activity
    {
        Button menuButton;
        Button searchButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.mainPage);

            menuButton = FindViewById<Button>(Resource.Id.menuButton);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
        }
        
    }
}