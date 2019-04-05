using System;

namespace Motorapido.Models
    {
    public class Cadastro
        {
        public string Id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string numeroTelefone { get; set; }
        public string senha { get; set; }
        public byte [] foto { get; set; }
        public Int64 codBinarioFoto { get; set; }
    }
        
    }