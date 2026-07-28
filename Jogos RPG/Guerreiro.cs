using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoRPG
{
    internal class Guerreiro : Personagem
    {
        private static Random rng = new Random();

        public Guerreiro(string nome) : base(nome, vidaMaxima: 120, ataqueBase: 15) { }

        public override ResultadoAcao Agir()
        {
            bool golpePoderoso = rng.Next(0, 100) < 25;
            int dano = golpePoderoso ? AtaqueBase * 2 : AtaqueBase;
            string desc = golpePoderoso
                ? $"{Nome} desfere um GOLPE PODEROSO! ({dano} de dano)"
                : $"{Nome} ataca com a espada. ({dano} de dano)";

            return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = dano, Descricao = desc };
        }
    }
}