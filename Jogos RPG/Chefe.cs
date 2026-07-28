using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Chefe.cs

namespace JogoRPG
{
    internal class Chefe : Personagem
    {
        private static Random rng = new Random();
        private bool jaEnfureceu = false;

        public Chefe(string nome) : base(nome, vidaMaxima: 220, ataqueBase: 18) { }

        public override ResultadoAcao Agir()
        {
            double percentualVida = (double)Vida / VidaMaxima;

            // Fase 3: desesperado (abaixo de 30% de vida) — ataque brutal, mas arriscado
            if (percentualVida < 0.30)
            {
                int dano = (int)(AtaqueBase * 2.2);
                string desc = $"{Nome} está DESESPERADO e ataca com tudo! ({dano} de dano)";
                return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = dano, Descricao = desc };
            }

            // Fase 2: enfurecido (abaixo de 60%) — acontece uma vez só, dá um "grito de guerra"
            if (percentualVida < 0.60 && !jaEnfureceu)
            {
                jaEnfureceu = true;
                AplicarEfeitoDeAtaque("Fúria do Chefe", 8, 999); // buff permanente pro resto da luta
                string desc = $"{Nome} solta um RUGIDO DE FÚRIA! Ataque aumentado permanentemente!";
                return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = 0, Descricao = desc };
            }

            // Fase 1: normal
            bool golpeForte = rng.Next(0, 100) < 20;
            int danoNormal = golpeForte ? AtaqueBase * 2 : AtaqueBase;
            string descNormal = golpeForte
                ? $"{Nome} desfere um golpe devastador! ({danoNormal} de dano)"
                : $"{Nome} ataca. ({danoNormal} de dano)";

            return new ResultadoAcao { Tipo = TipoAcao.Ataque, Valor = danoNormal, Descricao = descNormal };
        }
    }
}