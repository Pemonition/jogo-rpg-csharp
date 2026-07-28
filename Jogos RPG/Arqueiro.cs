using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace JogoRPG
{
    internal class Arqueiro : Personagem
    {
        private static Random rng = new Random();

        public Arqueiro(string nome) : base(nome, vidaMaxima: 90, ataqueBase: 12) { }

        public override ResultadoAcao Agir()
        {
            bool tiroCertero = rng.Next(0, 100) < 40;
            int dano = tiroCertero ? (int)(AtaqueBase * 1.5) : AtaqueBase;
            string desc = tiroCertero
                ? $"{Nome} acerta um TIRO CERTEIRO! ({dano} de dano)"
                : $"{Nome} dispara uma flecha. ({dano} de dano)";

            return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = dano, Descricao = desc };
        }
    }
}