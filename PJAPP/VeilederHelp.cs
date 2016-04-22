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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Android.Preferences;

namespace PJAPP
{
    [Activity(Label = "VeilederHelp")]
    public class VeilederHelp : Activity
    {
        TextView prosjektor1;
        TextView plasser1;
        TextView reserver1;

        TextView prosjektor2;
        TextView plasser2;
        TextView reserver2;

        Button reserverButton;
        ImageButton mainMenu;
        ImageButton menuButton;

        string student;
        string timeStamp;
        string LoginName;
        string rom;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.romDetalj);

            rom = "Rom 45";

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            student = prefs.GetString("userName", "");

            string name = Intent.GetStringExtra("name") ?? "Data not available";
            string FagID = Intent.GetStringExtra("FagID") ?? "Data not available";
            LoginName = Intent.GetStringExtra("LoginName") ?? "Data not available";

            reserver1 = FindViewById<TextView>(Resource.Id.reserver1);
            reserver2 = FindViewById<TextView>(Resource.Id.reserver2);
            plasser1  = FindViewById<TextView>(Resource.Id.plasser1);
            plasser2 = FindViewById<TextView>(Resource.Id.plasser2);
            prosjektor1 = FindViewById<TextView>(Resource.Id.prosjektor1);
            prosjektor2 = FindViewById<TextView>(Resource.Id.prosjektor2);

            reserverButton = FindViewById<Button>(Resource.Id.ReserverButton);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainMenu = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);

            prosjektor1.Text = "Navn: ";
            prosjektor2.Text = name;
            plasser1.Text = "Fag: ";
            plasser2.Text = FagID;

            reserver2.Visibility = ViewStates.Invisible;
            reserver1.Visibility = ViewStates.Invisible;

            mainMenu.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            reserverButton.Text = "Spør om hjelp";

            reserverButton.Click += delegate {
                DateTime currentTime = DateTime.Now;
                timeStamp = currentTime.ToString("MM.dd.yy HH:mm:ss");
                if(reqHelp())
                {
                    Toast msg = Toast.MakeText(this, "Noe gikk galt.", ToastLength.Long);
                    msg.Show();
                } else
                {
                    reserverButton.Text = "Forespørsel om hjelp sendt.";
                }
            };
        }

        public class data
        {
            public string veileder { get; set; }
            public string student { get; set; }
            public string rom { get; set; }
            public string timestamp { get; set; }
        }

        public bool reqHelp()
        {
            try
            {
                data DataObj = new data();
                DataObj.veileder = LoginName;
                DataObj.student = student;
                DataObj.rom = rom;
                DataObj.timestamp = timeStamp;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);

                string url = "http://pj3100.somee.com/reqHelp.php";

                HttpWebRequest newRequest = (HttpWebRequest)WebRequest.Create(url);

                newRequest.Method = "POST";

                string postData = JSONString;

                byte[] pdata = Encoding.UTF8.GetBytes(postData);

                newRequest.ContentType = "application/x-www-form-urlencoded";
                newRequest.ContentLength = pdata.Length;

                Stream myStream = newRequest.GetRequestStream();
                myStream.Write(pdata, 0, pdata.Length);

                WebResponse myResponse = newRequest.GetResponse();

                Stream responseStream = myResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(responseStream);

                string result = streamReader.ReadToEnd();


                streamReader.Close();
                responseStream.Close();
                myStream.Close();

                if (result.Equals("1"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (WebException ex)
            {
                string _exception = ex.ToString();
                Toast error = Toast.MakeText(this, _exception, ToastLength.Long);
                error.Show();
                Console.WriteLine("--->" + _exception);
                return false;
            }
        }
    }
}