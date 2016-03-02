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
    [Activity(Label = "RomDetalj")]
    public class RomDetalj : Activity
    {
        TextView romNavn;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.romDetalj);

            int minor = Intent.GetIntExtra("minor" , 0);
            int major = Intent.GetIntExtra("major", 0);
            string name = Intent.GetStringExtra("name") ?? "Data not available";
            string ID = Intent.GetStringExtra("ID") ?? "Data not available";

            romNavn = FindViewById<TextView>(Resource.Id.romNavn);
            romNavn.Text = name;
        }
        protected override void OnResume()
        {
            base.OnResume();
            
        }
    }
}