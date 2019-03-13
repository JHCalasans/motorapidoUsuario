using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Motorapido.Models;
using Motorapido.Views;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;

namespace Motorapido.ViewModels
    {
    public class HistóricoViewModel : BaseViewModel
        {
      
        public ObservableCollection<Historico> Items { get; set; }
        public Command LoadItemsCommand { get; set; }



        private string _myResultados;
        public string MyResultados
            {
            get
                {
                return _myResultados;
                }
            set
                {
                if (_myResultados != value)
                    {
                    _myResultados = value;
                    OnPropertyChanged();
                    }
                }
            }



        public HistóricoViewModel()
            {
            Title = "Histórico";
            Items = new ObservableCollection<Historico>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

          
            }

        async Task ExecuteLoadItemsCommand()
            {
            if (IsBusy)
                return;

            IsBusy = true;

            try
                {
                Items.Clear();

             

                string RestUrl = "http://104.248.186.97:8080/motorapido/ws/usuario/buscarHistorico";

                var uri = new Uri(string.Format(RestUrl, string.Empty));


                var json = JsonConvert.SerializeObject(80);

                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = null;


                var client = new HttpClient();

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    {

                    Console.WriteLine("Histórico com sucesso");

                    string input = await response.Content.ReadAsStringAsync();

                
                    List<dynamic> output = JsonConvert.DeserializeObject<List<dynamic>>(input);


                    MyResultados = output.Count.ToString();


                    for (int i = 0; i < output.Count; i++)
                        {

                        Historico historico = new Historico();

                        historico.dataChamada = output[i].dataChamada;
                        historico.situacao = output[i].situacao;
                        historico.destino = output[i].destino;
                        historico.origem = output[i].origem;
                    

                        try

                            {
                            historico.valor = output[i].valor;
                            }

                       catch
                            {
                            historico.valor = 0;
                            }

                        Items.Add(historico);

                           

                        }

                    }


                }
            catch (Exception ex)
                {
                Debug.WriteLine(ex);
                }
            finally
                {
                IsBusy = false;
                }
            }
        }
    }