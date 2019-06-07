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

namespace Motorapido.Views
    {

   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarPage : ContentPage
        {

     
        byte[] imagem = new byte [] { };

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
                foto = imagem
            };



            string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "imagem");
           
            File.WriteAllBytes(file, imagem);


              


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

                Preferences.Set("UserName", Nome.Text);

                Console.WriteLine("-------->", response.ToString());

                Application.Current.MainPage = new MainPage { Detail = new NavigationPage(new ViagensPage()) };


                }


            else

                {

                DisplayAlert("Ops!", response.StatusCode.ToString(), "Ok");

                }

            }


    

        public  CadastrarPage()
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
   