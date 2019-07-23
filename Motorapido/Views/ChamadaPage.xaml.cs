using System;
using System.Net.Http;
using System.Text;
using Motorapido.Models;
using Motorapido.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Motorapido.Views
    {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChamadaPage : ContentPage
        {
        public ChamadaPage(string latitudeOrigem, string longitudeOrigem, string latitudeDestino, string longitudeDestino, string logradouroOrigem, string bairroOrigem, string cepOrigem, string cidadeOrigem, string numeroOrigem, string logradouroDestino, string bairroDestino, string cepDestino, string cidadeDestino, string numeroDestino, string minutos, string kms)
            {

            InitializeComponent();



            LatitudeOrigem.Text = latitudeOrigem;

            LongitudeOrigem.Text = longitudeOrigem;


            LatitudeDestino.Text = latitudeDestino;

            LongitudeDestino.Text = longitudeDestino;


            Minutos.Text = minutos;

            Kms.Text =  kms;

      

            Minutos.Text = minutos;
            Kms.Text = kms;


            }


        async void Chamar_Clicked(object sender, System.EventArgs e)
            {
            string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/enviarChamada";

            var uri = new Uri(string.Format(RestUrl, string.Empty));


            Chamada data = new Chamada
                {


                codUsuario = int.Parse(Preferences.Get("UserId", "default_value")),
                cepOrigem = "teste",
                bairroOrigem = "teste",
                cidadeOrigem = "teste",
                logradouroOrigem = "teste",
                numeroOrigem = "teste",
                complementoOrigem = "teste",
                latitudeOrigem = "-10.916096",
                longitudeOrigem = "-37.048814",
                cepDestino = "teste",
                bairroDestino = "teste",
                cidadeDestino = "teste",
                logradouroDestino = "teste",
                numeroDestino = "teste",
                complementoDestino = "teste",
                latitudeDestino = "-10.016096",
                longitudeDestino = "-37.148814",
                observacao = Observação.Text



                };


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



                string input = await response.Content.ReadAsStringAsync();


                dynamic output = JsonConvert.DeserializeObject<dynamic>(input);




                RetornoChamada retornochamada = new RetornoChamada();

                //atualizar o que vem da des-serialização

                retornochamada.dataChamada = output.dataChamada;
                retornochamada.destino = output.destino;
                retornochamada.origem = output.origem;

                try
                    {
                    retornochamada.valor = output.valor;
                    }

                catch
                    {
                    retornochamada.valor = 0;
                    }

                retornochamada.codChamada = output.codChamada;
                retornochamada.placaVeiculo = output.placaVeiculo;
                retornochamada.corVeiculo = output.corVeiculo;
                retornochamada.modeloVeiculo = output.modeloVeiculo;
                retornochamada.nomeMotorista = output.nomeMotorista;


                await Navigation.PushAsync(new RetornoChamadaDetailPage(new RetornoChamadaDetailViewModel(retornochamada)));

                }

            else

                Console.WriteLine("------------------------>" + response.ReasonPhrase + " " + response.StatusCode);

            }
        }
    }

