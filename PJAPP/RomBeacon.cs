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
using EstimoteSdk;

namespace PJAPP
{
    public class RomBeacon
    {
        public int minor { get; set; }
        public int major { get; set; }
        //public double distance { get; set; }
        public string name { get; set; }
        public Java.Util.UUID UUID { get; set; }
    }
}