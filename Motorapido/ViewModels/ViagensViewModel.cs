﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Motorapido.Helpers;
using Motorapido.Models;
using Motorapido.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Motorapido.ViewModels
    {
    public class ViagensViewModel : INotifyPropertyChanged
        {
        public ICommand CalculateRouteCommand { get; set; }
        public ICommand UpdatePositionCommand { get; set; }

        public ICommand LoadRouteCommand { get; set; }
        public ICommand StopRouteCommand { get; set; }
        IGoogleMapsApiService googleMapsApi = new GoogleMapsApiService();

        public bool HasRouteRunning { get; set; }
        string _originLatitud;
        string _originLongitud;
        string _destinationLatitud;
        string _destinationLongitud;

        GooglePlaceAutoCompletePrediction _placeSelected;
        public GooglePlaceAutoCompletePrediction PlaceSelected
            {
            get
                {
                return _placeSelected;
                }
            set
                {
                _placeSelected = value;
                if (_placeSelected != null)
                    GetPlaceDetailCommand.Execute(_placeSelected);
                }
            }
        public ICommand FocusOriginCommand { get; set; }
        public ICommand GetPlacesCommand { get; set; }
        public ICommand GetPlaceDetailCommand { get; set; }

        public ObservableCollection<GooglePlaceAutoCompletePrediction> Places { get; set; }
        public ObservableCollection<GooglePlaceAutoCompletePrediction> RecentPlaces { get; set; } = new ObservableCollection<GooglePlaceAutoCompletePrediction>();

        public bool ShowRecentPlaces { get; set; }
        bool _isPickupFocused = true;

        string _pickupText;
        public string PickupText
            {
            get
                {
                return _pickupText;
                }
            set
                {
                _pickupText = value;
                if (!string.IsNullOrEmpty(_pickupText))
                    {
                    _isPickupFocused = true;
                    GetPlacesCommand.Execute(_pickupText);
                    }
                }

            }

        string _originText;
        public string OriginText
            {
            get
                {
                return _originText;
                }
            set
                {
                _originText = value;
                if (!string.IsNullOrEmpty(_originText))
                    {
                    _isPickupFocused = false;
                    GetPlacesCommand.Execute(_originText);
                    }
                }
            }

        public ICommand GetLocationNameCommand { get; set; }
        public bool IsRouteNotRunning
            {
            get
                {
                return !HasRouteRunning;
                }
            }



        private string _myKms;
        public string MyKms
            {
            get
                {
                return _myKms;
                }
            set
                {
                if (_myKms != value)
                    {
                    _myKms = value;

                    }
                }
            }

        private string _myTempo;
        public string MyTempo
            {
            get
                {
                return _myTempo;
                }
            set
                {
                if (_myTempo != value)
                    {
                    _myTempo = value;

                    }
                }
            }




        private bool _myMapa;
        public bool MyMapa
            {
            get
                {
                return _myMapa;
                }
            set
                {
                if (_myMapa != value)
                    {
                    _myMapa = value;

                    }
                }
            }

        public ViagensViewModel()


            {


            LoadRouteCommand = new Command(async () => await LoadRoute());
            StopRouteCommand = new Command(StopRoute);
            GetPlacesCommand = new Command<string>(async (param) => await GetPlacesByName(param));
            GetPlaceDetailCommand = new Command<GooglePlaceAutoCompletePrediction>(async (param) => await GetPlacesDetail(param));
            GetLocationNameCommand = new Command<Position>(async (param) => await GetLocationName(param));
            MyMapa = false;

            }

        public async Task LoadRoute()
            {
            var positionIndex = 1;

            var googleDirection = await googleMapsApi.GetDirections(_originLatitud.Replace(",", "."),
                _originLongitud.Replace(",", "."),
                _destinationLatitud.Replace(",", "."),
                _destinationLongitud.Replace(",", "."));


            if (googleDirection.Routes != null && googleDirection.Routes.Count > 0)
                {
                var positions = (Enumerable.ToList(PolylineHelper.Decode(googleDirection.Routes.First().OverviewPolyline.Points)));
                CalculateRouteCommand.Execute(positions);

                HasRouteRunning = true;

                MyMapa = true;

                var tempo = googleDirection.Routes[0].Legs[0].Duration.Text;


                var kms = googleDirection.Routes[0].Legs[0].Distance.Text;

                MyKms = kms;
                MyTempo = tempo;

                //Location tracking simulation
                //Device.StartTimer(TimeSpan.FromSeconds(1),() =>
                //{
                //    if(positions.Count>positionIndex && HasRouteRunning)
                //    {
                //    UpdatePositionCommand.Execute(positions[positionIndex]);
                //        positionIndex++;
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                //});
                }
            else
                {
                await Application.Current.MainPage.DisplayAlert(":(", "No route found", "Ok");

             
                }
         
            }
        public void StopRoute()
            {
            HasRouteRunning = false;
            MyMapa = false;

            }

        public async Task GetPlacesByName(string placeText)
            {

            var places = await googleMapsApi.GetPlaces(placeText);
            var placeResult = places.AutoCompletePlaces;


            if (placeResult != null && placeResult.Count > 0)
                {
                Places = new ObservableCollection<GooglePlaceAutoCompletePrediction>(placeResult);
                }

          

            ShowRecentPlaces = (placeResult == null || placeResult.Count == 0);
            }

        public async Task GetPlacesDetail(GooglePlaceAutoCompletePrediction placeA)
            {


            var place = await googleMapsApi.GetPlaceDetails(placeA.PlaceId);



            if (place != null)
                {
                if (_isPickupFocused)
                    {
                    PickupText = place.Name;
                    _originLatitud = $"{place.Latitude}";
                    _originLongitud = $"{place.Longitude}";
                    _isPickupFocused = false;
                    FocusOriginCommand.Execute(null);
                    }
                else
                    {
                    _destinationLatitud = $"{place.Latitude}";
                    _destinationLongitud = $"{place.Longitude}";

                    RecentPlaces.Add(placeA);

                    if (_originLatitud == _destinationLatitud && _originLongitud == _destinationLongitud)
                        {
                        await Application.Current.MainPage.DisplayAlert("Error", "Origin route should be different than Destination route", "Ok");
                        }
                    else
                        {

                        if (string.IsNullOrEmpty(_originLatitud) || string.IsNullOrEmpty(_originLongitud))
                            {
                            await Application.Current.MainPage.DisplayAlert("Error", "Enter and select Origin from the list", "Ok");
                            }

                        else
                            {

                            LoadRouteCommand.Execute(null);

                            var masterDetailPage =  Application.Current.MainPage as MasterDetailPage;


                            
                            await masterDetailPage.Detail.Navigation.PopAsync();


                            CleanFields();

                            }


                        }

                    }
                }


            }

        void CleanFields()
            {
            PickupText = OriginText = string.Empty;
            ShowRecentPlaces = true;
            PlaceSelected = null;
            }


        //Get place 
        public async Task GetLocationName(Position position)
            {
            try
                {
                var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                    {

                    // feature

                    if (placemark.FeatureName.IndexOf(placemark.Thoroughfare)>-1)

                        {

                        PickupText = placemark.FeatureName;


                        }

                    else
                        {

                        PickupText = placemark.Thoroughfare + " " + placemark.FeatureName;

                        }
                
                     
                    }
                else
                    {
                    PickupText = string.Empty;

                    }
                }
            catch (Exception ex)
                {
                Debug.WriteLine(ex.ToString());
                }
            }

        public event PropertyChangedEventHandler PropertyChanged;

        }
    }
