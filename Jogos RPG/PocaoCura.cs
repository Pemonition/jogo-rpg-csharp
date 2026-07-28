using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// PocaoCura.cs

namespace JogoRPG
{
    internal class PocaoCura : Item
    {
        public int QuantidadeCura { get; }

        public PocaoCura(int quantidadeCura) : base("Poção de Cura")
        {
            QuantidadeCura = quantidadeCura;
        }

        public override void Usar(Personagem alvo)
        {
            alvo.Curar(QuantidadeCura);
            Console.WriteLine($"{alvo.Nome} bebeu uma {Nome} e recuperou {QuantidadeCura} HP!");
        }
    }
}