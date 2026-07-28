using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoRPG
{
    internal enum TipoAcao { Ataque, Cura }

    internal class ResultadoAcao
    {
        public TipoAcao Tipo { get; set; }
        public int Valor { get; set; }
        public string Descricao { get; set; }
    }

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

        // Cada subclasse decide SUA ação — mas quem aplica o efeito é sempre o Program.cs
        public abstract ResultadoAcao Agir();

        public void ReceberDano(int dano)
        {
            Vida -= dano;
            if (Vida < 0) Vida = 0;
        }

        public void Curar(int quantidade)
        {
            Vida += quantidade;
            if (Vida > VidaMaxima) Vida = VidaMaxima;
        }

        public void AumentarAtaque(int quantidade)
        {
            AtaqueBase += quantidade;
        }

        public void MostrarStatus()
        {
            Console.WriteLine($"{Nome} [{GetType().Name}]: {Vida}/{VidaMaxima} HP");
        }
    }
}