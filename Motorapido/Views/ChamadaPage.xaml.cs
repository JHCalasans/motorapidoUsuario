using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Motorapido.Models;
using Motorapido.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Motorapido.Views
    {
    public partial class ChamadaPage : ContentPage
        {
        public ChamadaPage(string logradouroOrigem, string bairroOrigem, string cepOrigem, string cidadeOrigem, string numeroOrigem, string logradouroDestino, string bairroDestino, string cepDestino, string cidadeDestino, string numeroDestino, string minutos, string kms)
            {

            InitializeComponent();


            if (string.IsNullOrEmpty(numeroOrigem))

                {
                Origem.Text = bairroOrigem + numeroOrigem + " " + cepOrigem + " " + cidadeOrigem;


                }

            else
                {
                Origem.Text = bairroOrigem + " " + numeroOrigem + ", " + cepOrigem + " " + cidadeOrigem;



                }



            if (string.IsNullOrEmpty(numeroDestino))

                {
                Destino.Text = bairroDestino + numeroDestino + " " + cepDestino + " " + cidadeDestino;


                }

            else
                {
                Destino.Text = bairroDestino + " " + numeroDestino + ", " + cepDestino + " " + cidadeDestino;



                }

            LogradouroOrigem.Text = logradouroOrigem;
            LogradouroDestino.Text = logradouroDestino;


            Minutos.Text = minutos;
            Kms.Text = kms;


            }


        async void Chamar_Clicked(object sender, System.EventArgs e)
            {
            string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/enviarChamada";

            var uri = new Uri(string.Format(RestUrl, string.Empty));


            Chamada data = new Chamada
                {


                codUsuario = 172,
                cepOrigem = "teste",
                bairroOrigem = "teste",
                cidadeOrigem = "teste",
                logradouroOrigem = "teste",
                numeroOrigem = "teste",
                complementoOrigem = "teste",
                latitudeOrigem = "-10.940531",
                longitudeOrigem = "-37.057670",
                cepDestino = "teste",
                bairroDestino = "teste",
                cidadeDestino = "teste",
                logradouroDestino = "teste",
                numeroDestino = "teste",
                complementoDestino = "teste",
                latitudeDestino = "-10.921407",
                longitudeDestino = "-37.068590",
                observacao = Observação.Text



                };


            var json = JsonConvert.SerializeObject(data);

            var content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = null;


            var client = new HttpClient();

            response = await client.PostAsync(uri, content);

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
                Console.WriteLine("------------------------>" + response.ReasonPhrase + response.StatusCode);

            }
        }
    }

