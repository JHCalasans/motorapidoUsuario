﻿using System.Threading.Tasks;
using Motorapido.Models;

namespace Motorapido.Services
{
    public interface IGoogleMapsApiService
    {
        Task<GoogleDirection> GetDirections(string originLatitude, string originLongitude, string destinationLatitude, string destinationLongitude);
        Task<GooglePlaceAutoCompleteResult> GetPlaces(string text);
        Task<GooglePlace> GetPlaceDetails(string placeId);
    }
}
