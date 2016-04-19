
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace PJAPP
{
    [Activity(Label = "Menu")]
    public class Menu : Activity
    {
        private Button grupperom;
        private Button veileder;
        private Button minProfil;
        private Button loggUt;
        ImageButton mainPage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menuDialog);


            grupperom = FindViewById<Button>(Resource.Id.grupperomButton);
            veileder = FindViewById<Button>(Resource.Id.veilederButton);
            minProfil = FindViewById<Button>(Resource.Id.minProfilButton);
            loggUt = FindViewById<Button>(Resource.Id.loggUtButton);
            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);

            grupperom.Click += delegate
            {
                StartActivity(typeof(Grupperom));
            };

            veileder.Click += delegate
            {
                StartActivity(typeof(veilederSide));
            };

            minProfil.Click += delegate
            {
                StartActivity(typeof(profilSide));
            };

            loggUt.Click += delegate
            {
                StartActivity(typeof(MainActivity));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };
        }
    }
}