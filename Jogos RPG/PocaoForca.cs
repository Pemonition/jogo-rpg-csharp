using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JogoRPG
{
    internal class PocaoForca : Item
    {
        public int Bonus { get; private set; }

        public PocaoForca(int bonus) : base("Poção de Força")
        {
            Bonus = bonus;
        }

        public override void Usar(Personagem alvo)
        {
            alvo.AumentarAtaque(Bonus);
            Console.WriteLine($"{alvo.Nome} bebeu uma {Nome}! Ataque +{Bonus} pelo resto da batalha!");
        }
    }
}