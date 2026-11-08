using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivroCaixa.Classes
{
    public class Movimentacao
    {
        public int Id { get; set; }

        public DateTime DataMovimentacao { get; set; }

        public string Descricao { get; set; }

        // C = Crédito | D = Débito
        public string Tipo { get; set; }

        public decimal Valor { get; set; }

        public int UsuarioId
        {
            get; set;
        }
    }
}