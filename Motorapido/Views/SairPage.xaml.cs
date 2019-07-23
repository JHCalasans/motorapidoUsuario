using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
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

            Preferences.Set("UserId", "default_value");


            Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new LoginPage()) };





            }
        }
    }
