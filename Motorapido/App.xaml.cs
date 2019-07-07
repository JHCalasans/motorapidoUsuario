using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using Motorapido.Views;
using System.Xml.Linq;
using Plugin.Media;
using Com.OneSignal;
using Device = Xamarin.Forms.Device;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Motorapido
    {
    public partial class App : Application
        {

        Location location;
        public App()
            {
            InitializeComponent();


            CrossMedia.Current.Initialize();



            OneSignal.Current.StartInit("0df18d37-de4c-4705-87cb-c1d2f3c789d1")
             .EndInit();



            if (Preferences.Get("Cadastrado", "default_value") != "true")

                {

                // para Modal ......... MainPage = new CadastrarPage();

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new CadastrarPage()) };
                }

            else

                {



                if (Preferences.Get("Autenticado", "default_value") != "true")

                    {

                    // para Modal ......... MainPage = new CadastrarPage();

                    Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new LoginPage()) };
                    }


                else

                    {



                    Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };


                    }
                }
            }

        protected async override void OnStart()
            {
            // Handle when your app starts

            AppCenter.Start("android=5dca6bb3-5142-41a7-bb54-23790a0fcfd6;"
            + "uwp={Your UWP App secret here};"
            + "ios={Your iOS App secret here}", typeof(Analytics), typeof(Crashes));

            await GPS();




            }




        async Task GPS()
            {


            try
                {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                location = await Geolocation.GetLocationAsync(request);
                }

            catch
                {

                }
            }




        protected override void OnSleep()
            {
            // Handle when your app sleeps
            }

        protected override void OnResume()
            {
            // Handle when your app resumes
            }


        }
    }
