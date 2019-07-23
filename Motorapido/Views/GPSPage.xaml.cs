
using Xamarin.Forms;


#if __ANDROID__
using Android.Content;
using Android.Locations;

#endif
using Plugin.Permissions;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GPSPage : ContentPage
        {
        public GPSPage()
            {
            InitializeComponent();
            }


        void Definições_Clicked(object sender, System.EventArgs e)
            {


            if (Device.RuntimePlatform == Device.iOS)

                {




                CrossPermissions.Current.OpenAppSettings();
            


                }


            if (Device.RuntimePlatform == Device.Android)

                {


#if __ANDROID__

            
                Android.App.Application.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionApplicationSettings));

#endif




                }


     

            }





        }
    }
