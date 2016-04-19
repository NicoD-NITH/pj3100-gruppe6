
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace PJAPP
{
    [Activity(Label = "profilSide")]
    public class profilSide : Activity
    {
        ImageButton mainPage;
        ImageButton menuButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.profilSide);

            menuButton = FindViewById<ImageButton>(Resource.Id.menuButton);
            mainPage = FindViewById<ImageButton>(Resource.Id.westerdalsLogo);
            menuButton.Click += delegate
            {
                StartActivity(typeof(Menu));
            };
            mainPage.Click += delegate
            {
                StartActivity(typeof(MainPage));
            };

        }
    }
}