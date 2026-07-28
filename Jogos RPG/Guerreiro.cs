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

        public Guerreiro(string nome) : base(nome, vidaMaxima: 120, ataqueBase: 15)
        {
        }

        public override int Atacar()
        {
            bool golpePoderoso = rng.Next(0, 100) < 25; // 25% de chance

            if (golpePoderoso)
            {
                int dano = AtaqueBase * 2;
                Console.WriteLine($"{Nome} desfere um GOLPE PODEROSO! ({dano} de dano)");
                return dano;
            }

            Console.WriteLine($"{Nome} ataca com a espada. ({AtaqueBase} de dano)");
            return AtaqueBase;
        }
    }
}
