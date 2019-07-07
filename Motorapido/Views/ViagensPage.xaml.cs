using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net;


using System.Xml.Linq;
using System.Linq;



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

            if (string.IsNullOrEmpty(Origin)
            || string.IsNullOrEmpty(Destination)
            || string.IsNullOrEmpty(originid)
            || string.IsNullOrEmpty(destinationid)
            || string.IsNullOrEmpty(minutos)
            || string.IsNullOrEmpty(kms))

                {

                DisplayAlert("Ops!", "Introduza a Origem e o Destino", "OK");

                }


            else

                {

                string address1 = "https://maps.googleapis.com/maps/api/place/details/xml?placeid="

                   + originid + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s" + "&language=pt-BR";

                WebRequest request1 = WebRequest.Create(address1);
                WebResponse response1 = await request1.GetResponseAsync();
                XDocument xdoc1 = XDocument.Load(response1.GetResponseStream());


                //Location details

                // !!!!! try catch de outros componentes também 


                var cepOrigem = "";

                try
                    {

                    cepOrigem = xdoc1.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "postal_code").Parent.Element("long_name").Value;

                    }
                catch
                    {

                    cepOrigem = "";

                    }


                var cidadeOrigem = "";

                try
                    {


                    cidadeOrigem = xdoc1.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "locality").Parent.Element("long_name").Value;


                    }
                catch
                    {

                    cidadeOrigem = "";

                    }




                var numeroOrigem = "";

                try
                    {

                    numeroOrigem = xdoc1.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "street_number").Parent.Element("long_name").Value;


                    }
                catch
                    {

                    numeroOrigem = "";


                    }



                var bairroOrigem = "";

                try
                    {

                    bairroOrigem = xdoc1.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "route").Parent.Element("long_name").Value;


                    }
                catch
                    {

                    bairroOrigem = "";


                    }




                XElement result1 = xdoc1.Element("PlaceDetailsResponse").Element("result");
                XElement name1 = result1.Element("name");

                var logradouroOrigem = name1.Value;




                string address2 = "https://maps.googleapis.com/maps/api/place/details/xml?placeid="

                + destinationid + "&key=AIzaSyCXnSw7uj9P9oZIc_7c74peSmkmkYU1O5s" + "&language=pt-BR";

                WebRequest request2 = WebRequest.Create(address2);
                WebResponse response2 = await request2.GetResponseAsync();
                XDocument xdoc2 = XDocument.Load(response2.GetResponseStream());



                var cepDestino = "";

                try
                    {


                    cepDestino = xdoc2.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "postal_code").Parent.Element("long_name").Value;

                    }
                catch
                    {

                    cepDestino = "";


                    }



                var cidadeDestino = "";

                try
                    {


                    cidadeDestino = xdoc2.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "locality").Parent.Element("long_name").Value;

                    }

                catch
                    {

                    cidadeDestino = "";

                    }


                var numeroDestino = "";

                try
                    {

                    numeroDestino = xdoc2.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "street_number").Parent.Element("long_name").Value;


                    }
                catch
                    {

                    numeroDestino = "";


                    }



                var bairroDestino = "";

                try
                    {
                    bairroDestino = xdoc2.Descendants("address_component").Descendants("type").FirstOrDefault(x => x.Value == "route").Parent.Element("long_name").Value;


                    }
                catch
                    {

                    bairroDestino = "";


                    }



                XElement result2 = xdoc2.Element("PlaceDetailsResponse").Element("result");
                XElement name2 = result2.Element("name");
                var logradouroDestino = name2.Value;



                await Navigation.PushAsync(new ChamadaPage(logradouroOrigem, bairroOrigem, cepOrigem, cidadeOrigem, numeroOrigem, logradouroDestino, bairroDestino, cepDestino, cidadeDestino, numeroDestino, minutos, kms));


                }

            }

        Xamarin.Essentials.Location location = null;


        public ViagensPage()

            {



            InitializeComponent();



            }



        protected async override void OnAppearing()
            {
            base.OnAppearing();



            if (Preferences.Get("Cadastrado", "default_value") == "true")



                actInd.IsVisible = true;
                actInd.IsRunning = true;

            await GPS();

            if (location == null)




                {

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new GPSPage()) };



                }


            else

                {




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

        }
    }