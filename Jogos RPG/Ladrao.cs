using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Ladrao.cs — quinto personagem: no golpe crítico, também enfraquece o alvo

namespace JogoRPG
{
    internal class Ladrao : Personagem
    {
        private static Random rng = new Random();

        public Ladrao(string nome) : base(nome, vidaMaxima: 85, ataqueBase: 13) { }

        public override ResultadoAcao Agir()
        {
            bool golpeFurtivo = rng.Next(0, 100) < 35;
            int dano = golpeFurtivo ? (int)(AtaqueBase * 1.4) : AtaqueBase;
            string desc = golpeFurtivo
                ? $"{Nome} desfere um GOLPE FURTIVO! ({dano} de dano, alvo fica enfraquecido)"
                : $"{Nome} ataca com a adaga. ({dano} de dano)";

            return new ResultadoAcao
            {
                Tipo = TipoAcao.Ataque,
                Valor = dano,
                Descricao = desc,
                AplicaDebuff = golpeFurtivo,
            };
        }
    }
}