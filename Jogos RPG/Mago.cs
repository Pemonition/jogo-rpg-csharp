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

        public Mago(string nome) : base(nome, vidaMaxima: 80, ataqueBase: 20)
        {
        }

        public override int Atacar()
        {
            bool criticoMagico = rng.Next(0, 100) < 30; // 30% de chance

            if (criticoMagico)
            {
                int dano = (int)(AtaqueBase * 1.8);
                Console.WriteLine($"{Nome} conjura uma BOLA DE FOGO CRÍTICA! ({dano} de dano)");
                return dano;
            }

            Console.WriteLine($"{Nome} lança um raio arcano. ({AtaqueBase} de dano)");
            return AtaqueBase;
        }
    }
}
