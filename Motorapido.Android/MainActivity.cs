using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using ImageCircle.Forms.Plugin.Droid;
using Com.OneSignal;
using Plugin.Permissions;
using Xamarin.Forms.GoogleMaps.Android;

namespace Motorapido.Droid
    {
    [Activity(Label = "Motorapido", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation , ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        {


 public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
            {
          
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            }
     
       

        protected override void OnCreate(Bundle savedInstanceState)
            {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            
            OneSignal.Current.StartInit("0df18d37-de4c-4705-87cb-c1d2f3c789d1")
                     .EndInit();


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
         

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

            CrossCurrentActivity.Current.Init(this, savedInstanceState);


            // Override default BitmapDescriptorFactory by your implementation. 

            var platformConfig = new PlatformConfig
                {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
                };

            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig); // initialize for Xamarin.Forms.GoogleMaps


            LoadApplication(new App());
            }
        }
    }