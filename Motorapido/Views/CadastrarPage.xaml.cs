using System;
using Motorapido.Models;
using Motorapido.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Xamarin.Essentials;
using Plugin.Media;
using System.Collections.Generic;
using Plugin.FileUploader;
using Plugin.FileUploader.Abstractions;
using System.IO;
using Com.OneSignal;
using System.Security.Cryptography;

namespace Motorapido.Views
    {


    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarPage : ContentPage
        {


        byte[] imagem = new byte[] { };


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

        async void Cadastrar_Clicked(object sender, System.EventArgs e)

            {
            bool flag = true;


            if (string.IsNullOrEmpty(Nome.Text))

                {

                NomeObrigatório.IsVisible = true;

                flag = false;

                }
            else
                {


                NomeObrigatório.IsVisible = false;



                }


            if (string.IsNullOrEmpty(Email.Text))

                {

                EmailObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {


                EmailObrigatório.IsVisible = false;



                }

            if (string.IsNullOrEmpty(Telefone.Text))

                {

                TelefoneObrigatório.IsVisible = true;

                flag = false;

                }

            else
                {


                TelefoneObrigatório.IsVisible = false;



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


         

            Cadastro data = new Cadastro
                {
                nome = Nome.Text,
                email = Email.Text,
                numeroTelefone = Telefone.Text,
                senha = HashPassword(Senha.Text),
                foto = imagem
                };



            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "imagem");

            File.WriteAllBytes(file, imagem);


            OneSignal.Current.IdsAvailable((id, token) => data.idPush = id);


            string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/cadastrar";

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

          

                Preferences.Set("Cadastrado", "true");

                Console.WriteLine("Cadastrado com sucesso");

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new LoginPage()) };


                }


            else

                {

                await DisplayAlert("Ops!", response.StatusCode.ToString(), "Ok");

                }

            }




        public CadastrarPage()
            {
            InitializeComponent();


            }



        async void pickPhoto_Clicked(object sender, System.EventArgs e)
            {


            try
                {

                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                    });


                if (file == null)
                    return;


                using (var memoryStream = new MemoryStream())

                    {

                    file.GetStream().CopyTo(memoryStream);

                    imagem = memoryStream.ToArray();

                    }

                Console.WriteLine("----->" + imagem.Length.ToString());

                //    filePath = file.Path;
                //    paths.Enqueue(filePath);

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    // file.Dispose();
                    return stream;
                });



                }
            catch
                {

                }


            }



        }
    }
