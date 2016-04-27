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
using Android.Preferences;

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
        TextView reservert2;
        TextView reservert1;
        Button reserverButton;
        string name;
        string time;
        DateTime currentDate;
        string bookingStamp;
        string student;

        int thisHour, thisMinute;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.romDetalj);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            student = prefs.GetString("userName", "");

            int minor = Intent.GetIntExtra("minor", 0);
            int major = Intent.GetIntExtra("major", 0);
            name = Intent.GetStringExtra("name") ?? "Data not available";
            string ID = Intent.GetStringExtra("ID") ?? "Data not available";

            int storrelseText = Intent.GetIntExtra("plasser", 0);
            string prosjektorText = Intent.GetStringExtra("prosjektor") ?? "Data not available";
            int isBookable = Intent.GetIntExtra("isBookable", 0);
            bookingStamp = Intent.GetStringExtra("bookingStamp") ?? "Data not available";

            romNavn = FindViewById<TextView>(Resource.Id.romNavn);
            plasser = FindViewById<TextView>(Resource.Id.plasser2);
            prosjektor = FindViewById<TextView>(Resource.Id.prosjektor2);
            reserver = FindViewById<TextView>(Resource.Id.reserver2);
            reservert2 = FindViewById<TextView>(Resource.Id.reservert2);
            reservert1 = FindViewById<TextView>(Resource.Id.reservert1);

            romNavn.Text = "" + name;
            plasser.Text = storrelseText.ToString();
            prosjektor.Text = prosjektorText.ToString();
            reservert2.Text = "Rommet var sist reservert i tre timer fra " + bookingStamp;

            currentDate = DateTime.Now;
            time = currentDate.ToString("MM.dd.yyyy HH:mm:ss");

            thisHour = DateTime.Now.Hour;
            thisMinute = DateTime.Now.Minute;

            reserverButton = FindViewById<Button>(Resource.Id.ReserverButton);
            reserverButton.Visibility = ViewStates.Invisible;

            if(isBookable == 0)
            {
                reserver.Text = "Nei";
                reservert1.Visibility = ViewStates.Gone;
                reservert2.Visibility = ViewStates.Gone;
            } else
            {
                reserver.Text = "Ja";
            }

            if (canBeBooked())
            {
                reserverButton.Text = "Reserver de neste tre timene.";
                reserverButton.Visibility = ViewStates.Visible;
            }
            else
            {
                reserverButton.Visibility = ViewStates.Invisible;
            }

            mainMenu = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);

            mainMenu.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

            reserverButton.Click += delegate
            {
                /*if(!timeSet)
                {
                    timePicker tPicker = timePicker.NewInstance(delegate (DateTime selectedTime)
                    {
                        time = selectedTime.ToString("MM.dd.yy HH:mm:ss");
                        timeSet = true;
                    });
                    tPicker.Show(FragmentManager, "Pick a time:");
                }*/

                if (RequestBooking())
                {
                    Toast msg2 = Toast.MakeText(this, "Rommet kan ikke bookes for øyeblikket", ToastLength.Long);
                    msg2.Show();
                }
                else
                {
                    if (thisMinute >= 10)
                    {
                        reserverButton.Text = "Rommet er booket i 3 timer til " + (thisHour + 3) + ":" + thisMinute + ".";
                    }
                    else
                    {
                        reserverButton.Text = "Rommet er booket i 3 timer til " + (thisHour + 3) + ":" + "0" + thisMinute + ".";
                    }
                }
            };
        }

       
        public class data
        {
            public string navn { get; set; }
            public string timestamp { get; set; }
        }


        private bool canBeBooked()
        {
            try
            {
                data DataObj = new data();
                DataObj.navn = name;
                DataObj.timestamp = time;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);

                string url = "http://pj3100.somee.com/bookRom.php";

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

        private bool RequestBooking()
        {
            try
            {
                data DataObj = new data();
                DataObj.navn = name;
                DataObj.timestamp = time;


                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);


                string url = "http://pj3100.somee.com/makeBooking.php";

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

                if (result.Equals("0"))
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
