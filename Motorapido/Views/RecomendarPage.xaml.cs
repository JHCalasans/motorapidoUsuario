using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Motorapido.ViewModels;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecomendarPage : ContentPage
        {
        RecomendarViewModel viewModel;

        public RecomendarPage()
            {
            InitializeComponent();


       

            BindingContext = viewModel = new RecomendarViewModel();

            }
        }
    }
