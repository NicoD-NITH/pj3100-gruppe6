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
    [Activity(Label = "veilederSide")]
    public class veilederSide : Activity
    {
        ImageButton menuButton;
        ImageButton mainPage;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.veileder);

            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);

            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
        }
    }
}