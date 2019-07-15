using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Motorapido.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Security.Cryptography;
using Com.OneSignal;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
        {


        private static string HashPassword(string str)
            {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
            }

        private static string GetStringFromHash(byte[] hash)
            {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                {
                result.Append(hash[i].ToString("X2"));
                }
            return result.ToString();
            }


        async void Login_Clicked(object sender, System.EventArgs e)
            {


            bool flag = true;





            if (string.IsNullOrEmpty(Email.Text))

                {

                EmailObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {

                EmailObrigatório.IsVisible = false;

                }

            if (string.IsNullOrEmpty(Senha.Text))

                {


                SenhaObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {


                SenhaObrigatório.IsVisible = false;



                }

            if (!flag) return;


            string umpush="";


            OneSignal.Current.IdsAvailable((id, token) => umpush = id);


            Login data = new Login
                {

                email = Email.Text,

                senha = HashPassword(Senha.Text),

                idPush = umpush


                };



            string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/login";

            var uri = new Uri(string.Format(RestUrl, string.Empty));


            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;


            var client = new HttpClient();

       

            try

                {

                response = await client.PostAsync(uri, content);


                }

            catch
                {


                await Application.Current.MainPage.DisplayAlert("Erro", "API não disponível ou sem conectividade Internet.", "OK");




                return;


                }

       


            if (response.IsSuccessStatusCode)
                {

             

                var respStr = await response.Content.ReadAsStringAsync();



                dynamic output = JsonConvert.DeserializeObject<dynamic>(respStr);




                Preferences.Set("Autenticado", "true");

                Preferences.Set("UserName", output.nome.ToString());


                Preferences.Set("UserId", output.codigo.ToString());

                Console.WriteLine("Logado com sucesso");

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };




                }
            else
                {

                await DisplayAlert("Ops!", response.StatusCode.ToString(), "Ok");




                }


            }

        public LoginPage()
            {
            InitializeComponent();


        

            }


        protected async override void OnAppearing()
            {
            base.OnAppearing();


            if (Preferences.Get("Cadastrado", "default_value") != "true")

                {

                // para Modal ......... MainPage = new CadastrarPage();

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new CadastrarPage()) };
                }




            }
        }
    }
