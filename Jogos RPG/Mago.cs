using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace JogoRPG
{
    internal class Mago : Personagem
    {
        private static Random rng = new Random();

        public Mago(string nome) : base(nome, vidaMaxima: 80, ataqueBase: 20) { }

        public override ResultadoAcao Agir()
        {
            bool criticoMagico = rng.Next(0, 100) < 30;
            int dano = criticoMagico ? (int)(AtaqueBase * 1.8) : AtaqueBase;
            string desc = criticoMagico
                ? $"{Nome} conjura uma BOLA DE FOGO CRÍTICA! ({dano} de dano)"
                : $"{Nome} lança um raio arcano. ({dano} de dano)";

            return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = dano, Descricao = desc };
        }
    }
}