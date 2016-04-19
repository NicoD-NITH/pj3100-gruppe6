using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace PJAPP
{
    [Activity(Label = "RomDetalj")]
    public class RomDetalj : Activity
    {
        ImageButton mainMenu;
        ImageButton menuButton;
        TextView romNavn;
        TextView plasser;
        TextView prosjektor;
        TextView reserver;
        Button reserverButton;
        string name;
        string time;
        DateTime currentDate;

        /*private WebClient roomClient;
        private Uri servURL;*/

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.romDetalj);

            int minor = Intent.GetIntExtra("minor", 0);
            int major = Intent.GetIntExtra("major", 0);
            name = Intent.GetStringExtra("name") ?? "Data not available";
            string ID = Intent.GetStringExtra("ID") ?? "Data not available";

            int storrelseText = Intent.GetIntExtra("plasser", 0);
            string prosjektorText = Intent.GetStringExtra("prosjektor") ?? "Data not available";
            int isBookable = Intent.GetIntExtra("isBookable", 0);

            romNavn = FindViewById<TextView>(Resource.Id.romNavn);
            plasser = FindViewById<TextView>(Resource.Id.plasser2);
            prosjektor = FindViewById<TextView>(Resource.Id.prosjektor2);
            reserver = FindViewById<TextView>(Resource.Id.reserver2);
            romNavn.Text = name;
            plasser.Text = storrelseText.ToString();
            prosjektor.Text = prosjektorText.ToString();


            currentDate = DateTime.Now;
            time = currentDate.ToString("MM.dd.yyyy HH:mm:ss");

            reserverButton = FindViewById<Button>(Resource.Id.ReserverButton);
            reserverButton.Visibility = ViewStates.Invisible;

            if (SendToPhp())
            {
                reserver.Text = "Ja";
                reserverButton.Visibility = ViewStates.Visible;
            }
            else
            {
                reserver.Text = "Nei";
                reserverButton.Visibility = ViewStates.Invisible;
            }

            mainMenu = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);

            mainMenu.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };


        }
        public class data
        {
            public string navn { get; set; }
            public string timestamp { get; set; }
        }

        private bool SendToPhp()
        {
            try
            {
                data DataObj = new data();
                DataObj.navn = name;
                DataObj.timestamp = time;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);


                string url = "http://pj3100.somee.com/BookRomV2.php";

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
