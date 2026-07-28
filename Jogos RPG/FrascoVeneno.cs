using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// FrascoVeneno.cs — novo item, usado NO INIMIGO

namespace JogoRPG
{
    internal class FrascoVeneno : Item
    {
        public int DanoPorTurno { get; }
        public int Duracao { get; }

        public FrascoVeneno(int danoPorTurno, int duracao) : base("Frasco de Veneno")
        {
            DanoPorTurno = danoPorTurno;
            Duracao = duracao;
            AlvoEhInimigo = true;
        }

        public override void Usar(Personagem alvo)
        {
            alvo.AplicarVeneno(DanoPorTurno, Duracao);
            Console.WriteLine($"{alvo.Nome} foi envenenado! ({DanoPorTurno} de dano por {Duracao} turnos)");
        }
    }
}