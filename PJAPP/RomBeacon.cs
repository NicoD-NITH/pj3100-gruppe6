namespace PJAPP
{
    public class RomBeacon
    {
        public string BeaconUUID { get; set; }
        public int BeaconMajor { get; set; }
        public int BeaconMinor { get; set; } 
        public string RomNavn { get; set; }
        public string HarProsjektor { get; set; }
        public int Plasser { get; set; }
        public int IsBookable { get; set; }
        public string bookingStamp { get; set; }
        public double distance { get; set; }
    }
}