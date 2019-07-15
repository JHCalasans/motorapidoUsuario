using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using UIKit;
using Xamarin.Essentials;

namespace Motorapido.iOS
    {
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
        {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //


        // The following Exports are needed to run OneSignal in the iOS Simulator.
        //   The simulator doesn't support push however this prevents a crash due to a Xamarin bug
        //   https://bugzilla.xamarin.com/show_bug.cgi?id=52660
        [Export("oneSignalApplicationDidBecomeActive:")]
        public void OneSignalApplicationDidBecomeActive(UIApplication application)
            {
            // Remove line if you don't have a OnActivated method.
            OnActivated(application);
            }

        [Export("oneSignalApplicationWillResignActive:")]
        public void OneSignalApplicationWillResignActive(UIApplication application)
            {
            // Remove line if you don't have a OnResignActivation method.
            OnResignActivation(application);
            }

        [Export("oneSignalApplicationDidEnterBackground:")]
        public void OneSignalApplicationDidEnterBackground(UIApplication application)
            {
            // Remove line if you don't have a DidEnterBackground method.
            DidEnterBackground(application);
            }

        [Export("oneSignalApplicationWillTerminate:")]
        public void OneSignalApplicationWillTerminate(UIApplication application)
            {
            // Remove line if you don't have a WillTerminate method.
            WillTerminate(application);
            }

        // Note: Similar exports are needed if you add other AppDelegate overrides.




        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
            {

            // SpeakNow();




            global::Xamarin.Forms.Forms.Init();

            Xamarin.FormsGoogleMaps.Init(Constants.GoogleMapsApiKey);

            ImageCircleRenderer.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
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

        }
    }
