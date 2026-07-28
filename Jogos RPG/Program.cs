using System;
using System.Collections.Generic;

namespace JogoRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== ESCOLHA SEU PERSONAGEM =====");
            Console.WriteLine("1 - Guerreiro (mais vida, ataque físico)");
            Console.WriteLine("2 - Mago (menos vida, ataque mágico forte)");
            Console.WriteLine("3 - Arqueiro (vida média, ataques frequentes)");
            Console.Write("Escolha: ");

            Personagem jogador = CriarPersonagem(LerOpcao(), "Você");

            // Inimigo sorteado aleatoriamente entre os 3 tipos
            Random rng = new Random();
            int tipoInimigo = rng.Next(1, 4);
            Personagem inimigo = CriarPersonagem(tipoInimigo, "Inimigo");

            Console.WriteLine($"\nUm(a) {inimigo.GetType().Name} selvagem apareceu!\n");

            int turno = 1;
            while (jogador.EstaVivo && inimigo.EstaVivo)
            {
                Console.WriteLine($"--- Turno {turno} ---");

                int danoJogador = jogador.Atacar();
                inimigo.ReceberDano(danoJogador);

                if (!inimigo.EstaVivo)
                {
                    Console.WriteLine($"\n{inimigo.Nome} foi derrotado! Você venceu!");
                    break;
                }

                int danoInimigo = inimigo.Atacar();
                jogador.ReceberDano(danoInimigo);

                jogador.MostrarStatus();
                inimigo.MostrarStatus();
                Console.WriteLine();

                if (!jogador.EstaVivo)
                {
                    Console.WriteLine($"\n{jogador.Nome} foi derrotado! Você perdeu!");
                    break;
                }

                turno++;
            }
        }

        static int LerOpcao()
        {
            int opcao;
            while (!int.TryParse(Console.ReadLine(), out opcao) || opcao < 1 || opcao > 3)
            {
                Console.Write("Opção inválida, escolha 1, 2 ou 3: ");
            }
            return opcao;
        }

        // Aqui está o coração do polimorfismo: o mesmo método devolve
        // tipos DIFERENTES de Personagem, mas todos utilizáveis do mesmo jeito
        static Personagem CriarPersonagem(int tipo, string nome)
        {
            switch (tipo)
            {
                case 1: return new Guerreiro(nome);
                case 2: return new Mago(nome);
                case 3: return new Arqueiro(nome);
                default: throw new ArgumentException("Tipo inválido.");
            }
        }
    }
}
