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
    [Activity(Label = "Menu")]
    public class Menu : Activity
    {
        private Button grupperom;
        private Button veileder;
        private Button minProfil;
        private Button loggUt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.menuDialog);


            grupperom = FindViewById<Button>(Resource.Id.grupperomButton);
            veileder = FindViewById<Button>(Resource.Id.veilederButton);
            minProfil = FindViewById<Button>(Resource.Id.minProfilButton);
            loggUt = FindViewById<Button>(Resource.Id.loggUtButton);

            grupperom.Click += delegate
            {
                StartActivity(typeof(Grupperom));
            };

        }
    }
}