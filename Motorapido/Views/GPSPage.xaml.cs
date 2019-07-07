using System;
using System.Collections.Generic;

using Xamarin.Forms;


#if __ANDROID__
using Android.Content;
using Android.Locations;

#endif
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


namespace Motorapido.Views
    {
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
