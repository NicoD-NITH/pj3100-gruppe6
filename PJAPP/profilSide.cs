
using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Net;
using Newtonsoft.Json;
using Android.Preferences;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PJAPP
{
    [Activity(Label = "profilSide")]
    public class profilSide : Activity
    {
        public class User
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LoginName { get; set; }
            public string FagID { get; set; }

        }

        public List<User> user;
        ImageButton mainPage;
        ImageButton menuButton;
        TextView userName;
        TextView userLogin;
        TextView userCourse;

        static string sendText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.profilSide);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            sendText = prefs.GetString("userName", "");

            userName = FindViewById<TextView>(Resource.Id.userName);
            userCourse = FindViewById<TextView>(Resource.Id.userCourse);
            userLogin = FindViewById<TextView>(Resource.Id.userLoginName);

            if(SendToPhp() == true)
            {
                 userName.Text = user[0].FirstName + " " + user[0].LastName;
                 userLogin.Text = user[0].LoginName;
                 userCourse.Text = user[0].FagID;
            }
            else
            {
                Toast error = Toast.MakeText(this, "Could not get user info.", ToastLength.Long);
                error.Show();
            }


            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(Menu));
            };

        }

        public class data
        {
            public string navn { get; set; }
           
        }


        private bool SendToPhp()
        {
            try
            {
                data DataObj = new data();
                DataObj.navn = sendText;
                

                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);


                string url = "http://pj3100.somee.com/getUserInfo.php";

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

                user = JsonConvert.DeserializeObject<List<User>>(result);

                streamReader.Close();
                responseStream.Close();
                myStream.Close();
                
                if(user != null)
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