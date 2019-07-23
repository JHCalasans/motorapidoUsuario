
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Motorapido.Models;
using Motorapido.ViewModels;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RetornoChamadaDetailPage : ContentPage
        {

        RetornoChamadaDetailViewModel viewModel;

        public RetornoChamadaDetailPage(RetornoChamadaDetailViewModel viewModel)
            {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            }

        public RetornoChamadaDetailPage()
            {
            InitializeComponent();

            var retornochamada = new RetornoChamada
                {

                };

            viewModel = new RetornoChamadaDetailViewModel(retornochamada);
            BindingContext = viewModel;
            }
        }
    }
