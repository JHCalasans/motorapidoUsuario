using System;
using Motorapido.Models;
using Motorapido.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Xamarin.Essentials;



namespace Motorapido.Views
    {


 
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarPage : ContentPage
        {

     async void Cadastrar_Clicked(object sender, System.EventArgs e)
            {




         //  Preferences.Set("Cadastrado", "true");

         //   Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };


            Cadastro data = new Cadastro
                {
                nome = Nome.Text,
                email = Email.Text,
                numeroTelefone = Telefone.Text,
                senha = Senha.Text,
                };


            string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/cadastrar";

            var uri = new Uri(string.Format(RestUrl, string.Empty));


            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;


            var client = new HttpClient();

            response = await client.PostAsync(uri, content);

            if (response.IsSuccessStatusCode)
                {

                Console.WriteLine ("Cadastrado com sucesso");

                Preferences.Set("Cadastrado", "true");

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };


                }


            else

                {

                DisplayAlert("Ops!", response.StatusCode.ToString(), "Ok");

                }

            }


        void Cancelar_Clicked(object sender, System.EventArgs e)
            {

       
            }

        public  CadastrarPage()
            {
            InitializeComponent();

         
            }



            }
    }