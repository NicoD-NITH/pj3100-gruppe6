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
        TextView romNavn;

        TextView prosjektor1;
        TextView plasser1;
        TextView reserver1;
        TextView reservert1;

        TextView prosjektor2;
        TextView plasser2;
        TextView reserver2;
        TextView reservert2;

        ImageView pil;
        LinearLayout ln3;
        TextView velg;

        Button reserverButton;
        ImageButton mainMenu;
        ImageButton menuButton;

        Spinner romValg;

        string student;
        string timeStamp;
        string LoginName;
        string rom;

        public class romNavnListClass
        {
            string rom { get; set; }
            string navn { get; set; }
        }

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

            romNavn = FindViewById<TextView>(Resource.Id.romNavn);
            reserver1 = FindViewById<TextView>(Resource.Id.reserver1);
            reserver2 = FindViewById<TextView>(Resource.Id.reserver2);
            reservert1 = FindViewById<TextView>(Resource.Id.reservert1);
            reservert2 = FindViewById<TextView>(Resource.Id.reservert2);
            plasser1  = FindViewById<TextView>(Resource.Id.plasser1);
            plasser2 = FindViewById<TextView>(Resource.Id.plasser2);
            prosjektor1 = FindViewById<TextView>(Resource.Id.prosjektor1);
            prosjektor2 = FindViewById<TextView>(Resource.Id.prosjektor2);
            //pil = FindViewById<ImageView>(Resource.Id.imageView1);
            ln3 = FindViewById<LinearLayout>(Resource.Id.linearLayout3);
            velg = FindViewById<TextView>(Resource.Id.textView1);

            reserverButton = FindViewById<Button>(Resource.Id.ReserverButton);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainMenu = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);

            romNavn.Text = " Veilederinfo:";
            prosjektor1.Text = "Navn: ";
            prosjektor2.Text = name;
            plasser1.Text = "Fag: ";
            plasser2.Text = FagID;

            reserver2.Visibility = ViewStates.Invisible;
            reserver1.Visibility = ViewStates.Invisible;
            reservert2.Visibility = ViewStates.Invisible;
            reservert1.Visibility = ViewStates.Invisible;

            romValg = FindViewById<Spinner>(Resource.Id.romValg);
            romValg.Visibility = ViewStates.Visible;
            velg.Visibility = ViewStates.Visible;
            ln3.Visibility = ViewStates.Visible;
            
            romValg.ItemSelected += RomValg_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.rom_navn, Resource.Layout.spinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerItem);

            romValg.Adapter = adapter;

            mainMenu.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            reserverButton.Text = "Spør om hjelp";

            if(canGetHelp())
            {
                reserverButton.Visibility = ViewStates.Visible;
                romValg.Visibility = ViewStates.Visible;
                
            } else
            {
                reserverButton.Visibility = ViewStates.Invisible;
                romValg.Visibility = ViewStates.Invisible;
            }

            reserverButton.Click += delegate {
                DateTime currentTime = DateTime.Now;
                timeStamp = currentTime.ToString("MM.dd.yyyy HH:mm:ss");
                if(reqHelp() == true)
                {
                    reserverButton.Text = "Forespørsel om hjelp sendt.";
                } else
                {
                    Toast msg = Toast.MakeText(this, "Noe gikk galt.", ToastLength.Long);
                    msg.Show();
                    
                }
            };
        }

        private void RomValg_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            
            var s = sender as Spinner;
            rom = s.GetItemAtPosition(e.Position).ToString();
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

        public class canData
        {
            public string navn { get; set; }
        }

        public bool canGetHelp()
        {
            try
            {
                canData DataObj = new canData();
                DataObj.navn = student;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);

                string url = "http://pj3100.somee.com/canGetHelp.php";

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