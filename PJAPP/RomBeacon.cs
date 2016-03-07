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
        public string BeaconUUID { get; set; }
        public int BeaconMajor { get; set; }
        public int BeaconMinor { get; set; }
        
        public double distance { get; set; }
        public string RomNavn { get; set; }
    }
}