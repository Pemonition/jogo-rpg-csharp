using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// PocaoForca.cs — agora com duração, usando o sistema de efeitos
namespace JogoRPG
{
    internal class PocaoForca : Item
    {
        public int Bonus { get; }
        public int Duracao { get; }

        public PocaoForca(int bonus, int duracao) : base("Poção de Força")
        {
            Bonus = bonus;
            Duracao = duracao;
        }

        public override void Usar(Personagem alvo)
        {
            alvo.AplicarEfeitoDeAtaque("Força", Bonus, Duracao);
            Console.WriteLine($"{alvo.Nome} bebeu uma {Nome}! Ataque +{Bonus} por {Duracao} turnos!");
        }
    }
}