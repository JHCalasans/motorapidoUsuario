using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Motorapido.Views
    {
    public partial class SairPage : ContentPage
        {
        public SairPage()
            {


            InitializeComponent();


            }


       protected async override void OnAppearing()
           

              {


            base.OnAppearing();



            Preferences.Set("Autenticado", "false");

            Preferences.Set("UserName", "default_value");

       
            Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new LoginPage()) };





            }
        }
    }
