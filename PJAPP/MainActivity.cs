using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Android.Content;
using Android.Preferences;

namespace PJAPP
{
    [Activity(Label = "PJAPP", MainLauncher = true, Icon = "@drawable/icon")]

    public class MainActivity : Activity
    {
        private Button loggInnButton;

        static string sendText1;
        static string sendText2;

        protected override void OnCreate(Bundle bundle)
        {
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            loggInnButton = FindViewById<Button>(Resource.Id.loggInnButton);
            EditText editText1 = FindViewById<EditText>(Resource.Id.txtEmail);
            EditText editText2 = FindViewById<EditText>(Resource.Id.txtPassword);

            loggInnButton.Click += delegate
            {
                sendText1 = editText1.Text;
                sendText2 = editText2.Text;
                if (SendToPhp() == 1)
                {
                    StartActivity(typeof(Menu));
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("userName", editText1.Text);
                    editor.Apply();
                }
                else if(SendToPhp() == 2)
                {
                    Toast msg = Toast.MakeText(this, "Feil brukernavn.", ToastLength.Long);
                    msg.Show();
                } else
                {
                    Toast msg = Toast.MakeText(this, "Feil passord.", ToastLength.Long);
                    msg.Show();
                }
            };

        }
        public class data
        {
            public string df_text1 { get; set; }
            public string df_text2 { get; set; }
        }

        private int SendToPhp()
        {
            try
            {
                data DataObj = new data();
                DataObj.df_text1 = sendText1;
                DataObj.df_text2 = sendText2;

                string JSONString = JsonConvert.SerializeObject(DataObj, Formatting.None);


                string url = "http://pj3100.somee.com/cryptLogin.php";

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
                    return 1;
                }
                else if(result.Equals("2"))
                {
                    return 2;
                }
                else 
                {
                    return 0;
                }
            }
            catch (WebException ex)
            {
                string _exception = ex.ToString();
                Toast error = Toast.MakeText(this, _exception, ToastLength.Long);
                error.Show();
                Console.WriteLine("--->" + _exception);
                return 0;
            }
        }
    }
}