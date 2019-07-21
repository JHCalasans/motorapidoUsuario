using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Motorapido.ViewModels;
using Motorapido.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace Motorapido
    {



    public partial class ViagensPage : ContentPage
        {
        public static readonly BindableProperty CalculateCommandProperty =
            BindableProperty.Create(nameof(CalculateCommand), typeof(ICommand), typeof(ViagensPage), null, BindingMode.TwoWay);

        public ICommand CalculateCommand
            {
            get { return (ICommand)GetValue(CalculateCommandProperty); }
            set { SetValue(CalculateCommandProperty, value); }
            }

        public static readonly BindableProperty UpdateCommandProperty =
          BindableProperty.Create(nameof(UpdateCommand), typeof(ICommand), typeof(ViagensPage), null, BindingMode.TwoWay);

        public ICommand UpdateCommand
            {
            get { return (ICommand)GetValue(UpdateCommandProperty); }
            set { SetValue(UpdateCommandProperty, value); }
            }

     

        public bool IsLocationAvailable()
            {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
            }

        async Task StartListening()
            {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
            }

        private async void PositionChanged(object sender, PositionEventArgs e)
            {

            //If updating the UI, ensure you invoke on main thread
            var position = e.Position;
            //var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            //output += "\n" + $"Time: {position.Timestamp}";
            //output += "\n" + $"Heading: {position.Heading}";
            //output += "\n" + $"Speed: {position.Speed}";
            //output += "\n" + $"Accuracy: {position.Accuracy}";
            //output += "\n" + $"Altitude: {position.Altitude}";
            //output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";
            //await DisplayAlert("Move it!", output, "Done!");


            //Mover o icon na corrida
         


            }

        private void PositionError(object sender, PositionErrorEventArgs e)
            {
            Console.WriteLine(e.Error);
            //Handle event here for errors
            }

     



        //Posiçoes para a viagem

        Xamarin.Forms.GoogleMaps.Position inicio = new Xamarin.Forms.GoogleMaps.Position();

        Xamarin.Forms.GoogleMaps.Position fim = new Xamarin.Forms.GoogleMaps.Position();

        async void Chamada_Clicked(object sender, System.EventArgs e)
            {

            var vm = BindingContext as ViagensViewModel;


            await Navigation.PushAsync(new ChamadaPage(inicio.Latitude.ToString(), inicio.Longitude.ToString(), fim.Latitude.ToString(), fim.Longitude.ToString(), "logradouroOrigem", "bairroOrigem", "cepOrigem", "cidadeOrigem", "numeroOrigem", "logradouroDestino", "bairroDestino", "cepDestino", "cidadeDestino", "numeroDestino", vm.MyTempo, vm.MyKms));


            }


        public ViagensPage()
            {
            InitializeComponent();
            CalculateCommand = new Command<List<Xamarin.Forms.GoogleMaps.Position>>(Calculate);
            UpdateCommand = new Command<Xamarin.Forms.GoogleMaps.Position>(Update);
            GetActualLocationCommand = new Command(async () => await GetActualLocation());
           
            AddMapStyle();

            map.UiSettings.RotateGesturesEnabled = true;

            map.UiSettings.CompassEnabled = true;


            map.MyLocationEnabled = true;


            map.UiSettings.MyLocationButtonEnabled = true;

        
         
            }


        void AddMapStyle()
            {


#if __IOS__
            var resourcePrefix = "Motorapido.iOS.";
#endif
#if __ANDROID__
var resourcePrefix = "Motorapido.Droid.";
#endif

       
        
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ViagensPage)).Assembly;
  
            Stream stream = assembly.GetManifestResourceStream(resourcePrefix + "MapStyle.json");

            string styleFile;

            using (var reader = new System.IO.StreamReader(stream))
                {
                styleFile = reader.ReadToEnd();
                }

            map.MapStyle = MapStyle.FromJson(styleFile);

            }

        async void Update(Xamarin.Forms.GoogleMaps.Position position)
            {
            if (map.Pins.Count == 1 && map.Polylines != null && map.Polylines?.Count > 1)
                return;

            var cPin = map.Pins.FirstOrDefault();

            if (cPin != null)
                {
                cPin.Position = new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude);
                cPin.Icon = BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 100, HeightRequest = 100 });

                await map.MoveCamera(CameraUpdateFactory.NewPosition(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude)));
                var previousPosition = map?.Polylines?.FirstOrDefault()?.Positions?.FirstOrDefault();
                map.Polylines?.FirstOrDefault()?.Positions?.Remove(previousPosition.Value);
                }
            else
                {
                //END TRIP
                map.Polylines?.FirstOrDefault()?.Positions?.Clear();


                }


            }


        void Calculate(List<Xamarin.Forms.GoogleMaps.Position> list)
            {
          
            searchLayout.IsVisible = true;
            stopRouteButton.IsVisible = true;
            Chamada.IsVisible = true; 

            map.Polylines.Clear();
              var polyline = new Xamarin.Forms.GoogleMaps.Polyline();


            polyline.StrokeColor = Color.Green;

            polyline.StrokeWidth = (float)5; 



            foreach (var p in list)
                {
                polyline.Positions.Add(p);

                }
            map.Polylines.Add(polyline);


            var pin = new Xamarin.Forms.GoogleMaps.Pin
                {
                Type = PinType.Place,
                Position = new Xamarin.Forms.GoogleMaps.Position(polyline.Positions.First().Latitude, polyline.Positions.First().Longitude),
                Label = "First",
                Address = "First",
                Tag = string.Empty,
                Icon = BitmapDescriptorFactory.FromView(new Image() { Source = "ic_taxi.png", WidthRequest = 25, HeightRequest = 25 })

                };
            map.Pins.Add(pin);

            var pin1 = new Xamarin.Forms.GoogleMaps.Pin
                {
                Type = PinType.Place,
                Position = new Xamarin.Forms.GoogleMaps.Position(polyline.Positions.Last().Latitude, polyline.Positions.Last().Longitude),
                Label = "Last",
                Address = "Last",
                Tag = string.Empty
                };
            map.Pins.Add(pin1);



            //var medio = polyline.Positions.Count/ 2;


            //var pinmedio = new Xamarin.Forms.GoogleMaps.Pin
            //{
            //Type = PinType.Place,

            //Position = new Position(polyline.Positions[medio].Latitude, polyline.Positions[medio].Longitude),

            //Label = "Medio",
            //Address = "Medio",
            //Tag = string.Empty,

            //};

            //map.Pins.Add(pinmedio);




            inicio = pin.Position;

            fim = pin1.Position;

            map.MoveToRegion(MapSpan.FromPositions(list).WithZoom(0.85));

            }

        public async void OnEnterAddressTapped(object sender, EventArgs e)
            {
            await Navigation.PushAsync(new SearchPlacePage() { BindingContext = this.BindingContext }, false);

            }

        public async void Handle_Stop_Clicked(object sender, EventArgs e)
            {
          

            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();


            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;

            searchLayout.IsVisible = true;
            stopRouteButton.IsVisible = false;
            map.Polylines.Clear();
            map.Pins.Clear();

            }

        //Center map in actual location 
        protected async override void OnAppearing()
            {
   
            base.OnAppearing();

   
            GetActualLocationCommand.Execute(null);

            await StartListening();



            }

        public static readonly BindableProperty GetActualLocationCommandProperty =
            BindableProperty.Create(nameof(GetActualLocationCommand), typeof(ICommand), typeof(ViagensPage), null, BindingMode.TwoWay);

        public ICommand GetActualLocationCommand
            {
            get { return (ICommand)GetValue(GetActualLocationCommandProperty); }
            set { SetValue(GetActualLocationCommandProperty, value); }
            }

        async Task GetActualLocation()
            {
            try
                {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                var vm = BindingContext as ViagensViewModel;

                if (location != null && !vm.MyMapa)
                    {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(
                        new Xamarin.Forms.GoogleMaps.Position(location.Latitude, location.Longitude), Distance.FromMiles(0.3)));

                    }
                }
            catch (Exception ex)
                {
                await DisplayAlert("Error", "Unable to get actual location", "Ok");
                }
            }


        }
    }