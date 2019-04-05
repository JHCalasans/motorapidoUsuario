using System;

namespace Motorapido.Models
    {
    public class Chamada
        {
       
        public int codUsuario { get; set; }

        public string cepOrigem { get; set; }

        public string bairroOrigem { get; set; }

        public string cidadeOrigem { get; set; }

        public string logradouroOrigem { get; set; }

        public string numeroOrigem { get; set; }

        public string complementoOrigem { get; set; }

        public string latitudeOrigem { get; set; }

        public string longitudeOrigem { get; set; }



        public string cepDestino { get; set; }

        public string bairroDestino { get; set; }

        public string cidadeDestino { get; set; }

        public string logradouroDestino { get; set; }

        public string numeroDestino { get; set; }

        public string complementoDestino { get; set; }

        public string latitudeDestino { get; set; }

        public string longitudeDestino { get; set; }

        public string observacao { get; set; }



        }
    }