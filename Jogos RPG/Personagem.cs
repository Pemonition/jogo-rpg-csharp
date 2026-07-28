using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Personagem.cs


namespace JogoRPG
{
    internal enum TipoAcao { Ataque, Cura }
    internal enum TipoEfeito { BuffAtaque, DebuffAtaque, Veneno }

    internal class ResultadoAcao
    {
        public TipoAcao Tipo { get; set; }
        public int Valor { get; set; }
        public string Descricao { get; set; }
        public bool AplicaDebuff { get; set; } = false;
    }

    internal class Efeito
    {
        public TipoEfeito Tipo { get; set; }
        public int Valor { get; set; }          // dano por turno (só para Veneno)
        public int ImpactoAtaque { get; set; }   // quanto foi somado ao AtaqueBase (pode ser negativo)
        public int TurnosRestantes { get; set; }
        public string Nome { get; set; }
    }

    internal abstract class Personagem
    {
        public string Nome { get; set; }
        public int VidaMaxima { get; private set; }
        public int Vida { get; private set; }
        public int AtaqueBase { get; private set; }

        public bool EstaVivo => Vida > 0;

        private List<Efeito> efeitosAtivos = new List<Efeito>();

        protected Personagem(string nome, int vidaMaxima, int ataqueBase)
        {
            Nome = nome;
            VidaMaxima = vidaMaxima;
            Vida = vidaMaxima;
            AtaqueBase = ataqueBase;
        }

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

        // Único ponto de entrada pra mexer em AtaqueBase de fora — sempre registrando
        // o efeito, pra saber depois quanto reverter quando o tempo acabar.
        public void AplicarEfeitoDeAtaque(string nome, int impacto, int turnos)
        {
            AtaqueBase += impacto;
            efeitosAtivos.Add(new Efeito
            {
                Tipo = impacto >= 0 ? TipoEfeito.BuffAtaque : TipoEfeito.DebuffAtaque,
                ImpactoAtaque = impacto,
                TurnosRestantes = turnos,
                Nome = nome,
            });
        }

        public void AplicarVeneno(int danoPorTurno, int turnos)
        {
            efeitosAtivos.Add(new Efeito
            {
                Tipo = TipoEfeito.Veneno,
                Valor = danoPorTurno,
                TurnosRestantes = turnos,
                Nome = "Veneno",
            });
        }

        // Chamado no início do turno de CADA personagem: aplica dano de veneno
        // e derruba a contagem de todos os efeitos ativos.
        public void AplicarEfeitos()
        {
            foreach (var efeito in efeitosAtivos.ToList())
            {
                if (efeito.Tipo == TipoEfeito.Veneno)
                {
                    Console.WriteLine($"{Nome} sofre {efeito.Valor} de dano por veneno!");
                    ReceberDano(efeito.Valor);
                }
                efeito.TurnosRestantes--;
            }

            foreach (var expirado in efeitosAtivos.Where(e => e.TurnosRestantes <= 0).ToList())
            {
                if (expirado.ImpactoAtaque != 0)
                {
                    AtaqueBase -= expirado.ImpactoAtaque;
                    Console.WriteLine($"O efeito \"{expirado.Nome}\" em {Nome} acabou.");
                }
                efeitosAtivos.Remove(expirado);
            }
        }

        public void MostrarStatus()
        {
            Console.WriteLine($"{Nome} [{GetType().Name}]: {Vida}/{VidaMaxima} HP");
        }
    }
}