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

        public Arqueiro(string nome) : base(nome, vidaMaxima: 90, ataqueBase: 12)
        {
        }

        public override int Atacar()
        {
            bool tiroCertero = rng.Next(0, 100) < 40; // 40% de chance (mais frequente, menos dano extra)

            if (tiroCertero)
            {
                int dano = (int)(AtaqueBase * 1.5);
                Console.WriteLine($"{Nome} acerta um TIRO CERTEIRO! ({dano} de dano)");
                return dano;
            }

            Console.WriteLine($"{Nome} dispara uma flecha. ({AtaqueBase} de dano)");
            return AtaqueBase;
        }
    }
}