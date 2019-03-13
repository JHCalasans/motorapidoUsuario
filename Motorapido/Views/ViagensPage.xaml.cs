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
using Newtonsoft.Json;
using System.Net.Http;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViagensPage : ContentPage
        {

        async void Chamada_Clicked(object sender, System.EventArgs e)
            {


            string Origin = await Browser.EvaluateJavaScriptAsync("getOrigin()");
            string Destination = await Browser.EvaluateJavaScriptAsync("getDestination()");


            string originid = await Browser.EvaluateJavaScriptAsync("getOriginPlaceId()");
            string destinationid = await Browser.EvaluateJavaScriptAsync("getDestinationPlaceId()");   
            string minutos = await Browser.EvaluateJavaScriptAsync("getMinutos()");
            string kms = await Browser.EvaluateJavaScriptAsync("getKms()");


            Console.WriteLine("---->" + Origin + " " + originid);

            Console.WriteLine("---->" + Destination + " " + destinationid);

        

            // Validar os input's do origin-input e destination-input e places id's

            if (string.IsNullOrEmpty(Destination) || string.IsNullOrEmpty(Origin) || string.IsNullOrEmpty(originid) || string.IsNullOrEmpty(destinationid))

                {

                DisplayAlert("Ops!", "Introduza a Origem e o Destino", "OK");

                }


            else

                {

                string address1 = "https://maps.googleapis.com/maps/api/place/details/xml?placeid="

                   + originid + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s";

                WebRequest request1 = WebRequest.Create(address1);
                WebResponse response1 = await request1.GetResponseAsync();
                XDocument xdoc1 = XDocument.Load(response1.GetResponseStream());

                XElement result1 = xdoc1.Element("PlaceDetailsResponse").Element("result");
                XElement locationElement1 = result1.Element("formatted_address");


                string origem = locationElement1.Value;

                string address2 = "https://maps.googleapis.com/maps/api/place/details/xml?placeid="

                + destinationid + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s";

                WebRequest request2 = WebRequest.Create(address2);
                WebResponse response2 = await request2.GetResponseAsync();
                XDocument xdoc2 = XDocument.Load(response2.GetResponseStream());

                XElement result2 = xdoc2.Element("PlaceDetailsResponse").Element("result");
                XElement locationElement2 = result2.Element("formatted_address");


                string destino = locationElement2.Value;


                await Navigation.PushAsync(new ChamadaPage(origem, destino, minutos, kms));


                }

            }



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


            // Preferences.Set("latitude", location.Latitude.ToString().Replace(",", "."));
            string latitude = location.Latitude.ToString().Replace(",", ".");

            //  Preferences.Set("longitude", location.Longitude.ToString().Replace(",", "."));
            string longitude = location.Longitude.ToString().Replace(",", ".");

            string address = "https://maps.googleapis.com/maps/api/geocode/xml?latlng="

             + latitude + "," + longitude + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s";

            WebRequest request = WebRequest.Create(address);
            WebResponse response = await request.GetResponseAsync();
            XDocument xdoc = XDocument.Load(response.GetResponseStream());

            XElement result = xdoc.Element("GeocodeResponse").Element("result");
            XElement locationElement = result.Element("formatted_address");
            XElement placeidElement = result.Element("place_id");

        //    Preferences.Set("origem", locationElement.Value);
            string origem = locationElement.Value;

            //    Preferences.Set("placeid", placeidElement.Value);
            string placeid = placeidElement.Value;



            Browser.Source = "http://fferreira-001-site3.gtempurl.com/Index.html?latitude="
            + latitude + "&longitude="
            + longitude + "&origem="
            + origem
            + "&placeid=" + placeid;

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