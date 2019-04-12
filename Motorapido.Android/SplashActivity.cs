using System;

using Android.App;
using Android.Content;

using Android.OS;

using System.Threading.Tasks;

using Xamarin.Essentials;
using Android.Runtime;
using Android.Content.PM;

namespace Motorapido.Droid
    {

    [Activity(Label = "Motorapido", Theme = "@style/SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
            {
            base.OnCreate(savedInstanceState, persistentState);
          
         
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); 
            }

       

        // Launches the startup task
        protected override void OnStart()
            {


            base.OnStart();
          //  SpeakNow();

            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
            }

        public async Task SpeakNow()
            {
            var settings = new SpeechOptions()
                {
                Volume = (float).75,
                Pitch = (float)1.0
                };

            await TextToSpeech.SpeakAsync("Bem vindo ao app moto rápido.", settings);
            }


        async void SimulateStartup()
            {

            // await Task.Delay(5000); 

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }


            public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
            {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }

        }



    }
