using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoRPG
{
    internal abstract class Personagem
    {
        public string Nome { get; set; }
        public int VidaMaxima { get; private set; }
        public int Vida { get; private set; }
        public int AtaqueBase { get; protected set; }

        public bool EstaVivo => Vida > 0;

        protected Personagem(string nome, int vidaMaxima, int ataqueBase)
        {
            Nome = nome;
            VidaMaxima = vidaMaxima;
            Vida = vidaMaxima;
            AtaqueBase = ataqueBase;
        }

        // Cada subclasse é OBRIGADA a definir seu próprio jeito de atacar
        public abstract int Atacar();

        public void ReceberDano(int dano)
        {
            Vida -= dano;
            if (Vida < 0) Vida = 0;
        }

        public void MostrarStatus()
        {
            Console.WriteLine($"{Nome}: {Vida}/{VidaMaxima} HP");
        }
    }
}