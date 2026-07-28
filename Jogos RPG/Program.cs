using System;
using System.Collections.Generic;
// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace JogoRPG
{
    internal class Program
    {
        static Random rng = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("===== MONTE SEU TIME (2 personagens) =====");
            List<Personagem> timeJogador = new List<Personagem>
            {
                EscolherPersonagem("Herói 1"),
                EscolherPersonagem("Herói 2"),
            };

            List<Personagem> timeInimigo = new List<Personagem>
            {
                CriarPersonagem(rng.Next(1, 5), "Inimigo 1"),
                CriarPersonagem(rng.Next(1, 5), "Inimigo 2"),
            };

            List<Item> inventarioJogador = new List<Item>
            {
                new PocaoCura(25),
                new PocaoCura(25),
                new PocaoForca(5),
            };

            Console.WriteLine("\nSeu time enfrenta:");
            foreach (var i in timeInimigo)
                Console.WriteLine($" - {i.Nome} ({i.GetType().Name})");

            List<Personagem> ordemDeTurno = new List<Personagem>();
            ordemDeTurno.AddRange(timeJogador);
            ordemDeTurno.AddRange(timeInimigo);

            int turno = 1;
            while (TimeVivo(timeJogador) && TimeVivo(timeInimigo))
            {
                Console.WriteLine($"\n===== TURNO {turno} =====");

                foreach (var atual in ordemDeTurno)
                {
                    if (!atual.EstaVivo) continue;
                    if (!TimeVivo(timeJogador) || !TimeVivo(timeInimigo)) break;

                    if (timeJogador.Contains(atual))
                        TurnoJogador(atual, timeJogador, timeInimigo, inventarioJogador);
                    else
                        TurnoInimigo(atual, timeInimigo, timeJogador);
                }

                Console.WriteLine("\n-- Status --");
                foreach (var p in timeJogador.Concat(timeInimigo))
                    p.MostrarStatus();

                turno++;
            }

            Console.WriteLine(TimeVivo(timeJogador) ? "\n🏆 Seu time venceu!" : "\n💀 Seu time foi derrotado!");
        }

        static bool TimeVivo(List<Personagem> time) => time.Any(p => p.EstaVivo);

        static void TurnoJogador(Personagem atual, List<Personagem> timeJogador, List<Personagem> timeInimigo, List<Item> inventario)
        {
            Console.WriteLine($"\nTurno de {atual.Nome} ({atual.GetType().Name})");
            Console.WriteLine("1 - Agir (atacar/curar)  2 - Usar item");
            int opcao = LerOpcaoEntre(1, inventario.Any() ? 2 : 1);

            if (opcao == 2)
            {
                for (int i = 0; i < inventario.Count; i++)
                    Console.WriteLine($"{i + 1} - {inventario[i].Nome}");
                int escolha = LerOpcaoEntre(1, inventario.Count) - 1;

                var vivos = timeJogador.Where(p => p.EstaVivo).ToList();
                Console.WriteLine("Usar em quem?");
                for (int i = 0; i < vivos.Count; i++)
                    Console.WriteLine($"{i + 1} - {vivos[i].Nome} ({vivos[i].Vida}/{vivos[i].VidaMaxima} HP)");
                int alvoIdx = LerOpcaoEntre(1, vivos.Count) - 1;

                inventario[escolha].Usar(vivos[alvoIdx]);
                inventario.RemoveAt(escolha);
                return;
            }

            var resultado = atual.Agir();
            Console.WriteLine(resultado.Descricao);

            if (resultado.Tipo == TipoAcao.Ataque)
            {
                var inimigosVivos = timeInimigo.Where(p => p.EstaVivo).ToList();
                Console.WriteLine("Atacar quem?");
                for (int i = 0; i < inimigosVivos.Count; i++)
                    Console.WriteLine($"{i + 1} - {inimigosVivos[i].Nome} ({inimigosVivos[i].Vida}/{inimigosVivos[i].VidaMaxima} HP)");
                int alvoIdx = LerOpcaoEntre(1, inimigosVivos.Count) - 1;

                inimigosVivos[alvoIdx].ReceberDano(resultado.Valor);
            }
            else
            {
                var aliadosVivos = timeJogador.Where(p => p.EstaVivo).ToList();
                Console.WriteLine("Curar quem?");
                for (int i = 0; i < aliadosVivos.Count; i++)
                    Console.WriteLine($"{i + 1} - {aliadosVivos[i].Nome} ({aliadosVivos[i].Vida}/{aliadosVivos[i].VidaMaxima} HP)");
                int alvoIdx = LerOpcaoEntre(1, aliadosVivos.Count) - 1;

                aliadosVivos[alvoIdx].Curar(resultado.Valor);
            }
        }

        static void TurnoInimigo(Personagem atual, List<Personagem> timeInimigo, List<Personagem> timeJogador)
        {
            var resultado = atual.Agir();
            Console.WriteLine($"\n{resultado.Descricao}");

            if (resultado.Tipo == TipoAcao.Ataque)
            {
                var alvos = timeJogador.Where(p => p.EstaVivo).ToList();
                alvos[rng.Next(alvos.Count)].ReceberDano(resultado.Valor);
            }
            else
            {
                timeInimigo.Where(p => p.EstaVivo).OrderBy(p => p.Vida).First().Curar(resultado.Valor);
            }
        }

        static Personagem EscolherPersonagem(string nomeSlot)
        {
            Console.WriteLine($"\n{nomeSlot}:");
            Console.WriteLine("1-Guerreiro 2-Mago 3-Arqueiro 4-Curandeiro");
            int tipo = LerOpcaoEntre(1, 4);
            Console.Write("Nome do personagem: ");
            string nome = Console.ReadLine();
            return CriarPersonagem(tipo, nome);
        }

        static Personagem CriarPersonagem(int tipo, string nome)
        {
            switch (tipo)
            {
                case 1: return new Guerreiro(nome);
                case 2: return new Mago(nome);
                case 3: return new Arqueiro(nome);
                case 4: return new Curandeiro(nome);
                default: throw new ArgumentException("Tipo inválido.");
            }
        }

        static int LerOpcaoEntre(int min, int max)
        {
            int opcao;
            while (!int.TryParse(Console.ReadLine(), out opcao) || opcao < min || opcao > max)
                Console.Write($"Opção inválida, escolha entre {min} e {max}: ");
            return opcao;
        }
    }
}