using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Motorapido.Models;
using Motorapido.Views;
using Motorapido.ViewModels;
using Xamarin.Essentials;
using System.Net;



using System.Xml;

using System.Xml.Linq;
namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViagensPage : ContentPage
        {

        Location location;


        public ViagensPage()

            {



            InitializeComponent();



            }



        protected async override void OnAppearing()
            {
            base.OnAppearing();

        

            if (Preferences.Get("Cadastrado", "default_value") == "true")

                {
                actInd.IsVisible = true;
                actInd.IsRunning = true;
                }



            await GPS();
            

            Preferences.Set("latitude", location.Latitude.ToString().Replace(",", "."));
            Preferences.Set("longitude", location.Longitude.ToString().Replace(",", "."));

            string address = "https://maps.googleapis.com/maps/api/geocode/xml?latlng="

             + Preferences.Get("latitude", "default_value") + "," + Preferences.Get("longitude", "default_value") + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s";

            WebRequest request = WebRequest.Create(address);
            WebResponse response = await request.GetResponseAsync();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("formatted_address");
            XElement placeidElement = result.Element("place_id");

            Preferences.Set("origem", locationElement.Value);
            Preferences.Set("placeid", placeidElement.Value);




            Browser.Source = "http://fferreira-001-site3.gtempurl.com/Index.html?latitude="
            + Preferences.Get("latitude", "default_value") + "&longitude="
            + Preferences.Get("longitude", "default_value") + "&origem="
            + Preferences.Get("origem", "default_value")
            + "&placeid=" + Preferences.Get("placeid", "default_value");

            //  Browser.Reload();


            actInd.IsVisible = false;
            actInd.IsRunning = false;



            }

        async Task GPS()
            {


            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            location = await Geolocation.GetLocationAsync(request);

            }
        }
    }