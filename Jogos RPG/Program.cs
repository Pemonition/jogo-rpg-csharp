using System.Text.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JogoRPG
{
    internal enum ResultadoBatalha { Vitoria, Derrota, Fuga }

    internal class PersonagemSalvo
    {
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public int Vida { get; set; }
    }

    internal class JogoSalvo
    {
        public int Ouro { get; set; }
        public int Onda { get; set; }
        public List<PersonagemSalvo> Time { get; set; }
        public List<string> Inventario { get; set; }
    }

    internal class Program
    {
        static Random rng = new Random();
        const string ArquivoSave = "save.json";

        static void Main(string[] args)
        {
            List<Personagem> timeJogador;
            List<Item> inventarioJogador;
            int ouro;
            int onda;

            Console.WriteLine("1 - Novo jogo  2 - Carregar jogo salvo");
            int opcaoInicial = LerOpcaoEntre(1, 2);

            if (opcaoInicial == 2 && File.Exists(ArquivoSave))
            {
                CarregarJogo(out timeJogador, out inventarioJogador, out ouro, out onda);
            }
            else
            {
                if (opcaoInicial == 2)
                    Console.WriteLine("Nenhum save encontrado. Iniciando novo jogo.");

                timeJogador = new List<Personagem> { EscolherPersonagem("Herói 1"), EscolherPersonagem("Herói 2") };
                inventarioJogador = new List<Item> { new PocaoCura(25), new PocaoCura(25) };
                ouro = 50;
                onda = 1;
            }

            while (TimeVivo(timeJogador))
            {
                Console.WriteLine($"\n########## ONDA {onda} ##########");

                List<Personagem> timeInimigo;
                if (onda % 3 == 0)
                {
                    string[] nomesChefes = { "Rei Esqueleto", "Dragão Ancião", "Bruxa das Sombras", "Golem de Pedra" };
                    string nomeChefe = nomesChefes[rng.Next(nomesChefes.Length)];
                    timeInimigo = new List<Personagem> { new Chefe(nomeChefe) };
                    Console.WriteLine($"⚠️  Um CHEFE apareceu: {nomeChefe}!");
                }
                else
                {
                    timeInimigo = GerarInimigosComuns(onda);
                    Console.WriteLine("Inimigos:");
                    foreach (var i in timeInimigo)
                        Console.WriteLine($" - {i.Nome} ({i.GetType().Name})");
                }

                ResultadoBatalha resultadoBatalha = Batalha(timeJogador, timeInimigo, inventarioJogador);

                if (resultadoBatalha == ResultadoBatalha.Derrota) break;

                if (resultadoBatalha == ResultadoBatalha.Fuga)
                {
                    Console.WriteLine("Vocês fugiram! Tentem essa onda de novo quando estiverem prontos.");
                    continue;
                }

                int recompensa = (onda % 3 == 0) ? 80 + rng.Next(0, 30) : 30 + rng.Next(0, 20);
                ouro += recompensa;
                Console.WriteLine($"\nVocê venceu a onda {onda}! +{recompensa} de ouro (total: {ouro})");

                Console.WriteLine("\n1 - Ir à loja  2 - Continuar  3 - Salvar e sair");
                int op = LerOpcaoEntre(1, 3);
                if (op == 1) Loja(inventarioJogador, ref ouro);
                else if (op == 3)
                {
                    SalvarJogo(timeJogador, inventarioJogador, ouro, onda + 1);
                    return;
                }

                onda++;
            }

            Console.WriteLine(TimeVivo(timeJogador) ? "" : "\n💀 Fim de jogo — seu time foi derrotado.");
        }

        static List<Personagem> GerarInimigosComuns(int onda)
        {
            int tipo1 = rng.Next(1, 6);
            int tipo2;
            do
            {
                tipo2 = rng.Next(1, 6);
            } while (tipo1 == 4 && tipo2 == 4); // 4 = Curandeiro — nunca os dois juntos

            return new List<Personagem>
            {
                CriarPersonagem(tipo1, $"Inimigo {onda}-1"),
                CriarPersonagem(tipo2, $"Inimigo {onda}-2"),
            };
        }

        static ResultadoBatalha Batalha(List<Personagem> timeJogador, List<Personagem> timeInimigo, List<Item> inventarioJogador)
        {
            var ordemDeTurno = new List<Personagem>();
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

                    atual.AplicarEfeitos();
                    if (!atual.EstaVivo)
                    {
                        Console.WriteLine($"{atual.Nome} morreu por efeitos negativos!");
                        continue;
                    }

                    if (timeJogador.Contains(atual))
                    {
                        bool fugiu = TurnoJogador(atual, timeJogador, timeInimigo, inventarioJogador);
                        if (fugiu) return ResultadoBatalha.Fuga;
                    }
                    else
                    {
                        TurnoInimigo(atual, timeInimigo, timeJogador);
                    }
                }

                Console.WriteLine("\n-- Status --");
                foreach (var p in timeJogador.Concat(timeInimigo))
                    p.MostrarStatus();

                turno++;
            }

            return TimeVivo(timeJogador) ? ResultadoBatalha.Vitoria : ResultadoBatalha.Derrota;
        }

        static bool TimeVivo(List<Personagem> time) => time.Any(p => p.EstaVivo);

        static bool TurnoJogador(Personagem atual, List<Personagem> timeJogador, List<Personagem> timeInimigo, List<Item> inventario)
        {
            Console.WriteLine($"\nTurno de {atual.Nome} ({atual.GetType().Name})");
            Console.WriteLine("1 - Agir (atacar/curar)  2 - Usar item  3 - Fugir");
            int opcao = LerOpcaoEntre(1, 3);

            if (opcao == 3)
            {
                Console.WriteLine($"{atual.Nome} deu o sinal de retirada! O grupo foge da batalha.");
                return true;
            }

            if (opcao == 2)
            {
                if (!inventario.Any())
                {
                    Console.WriteLine("Você não tem itens! Turno perdido.");
                    return false;
                }

                for (int i = 0; i < inventario.Count; i++)
                    Console.WriteLine($"{i + 1} - {inventario[i].Nome}");
                int escolha = LerOpcaoEntre(1, inventario.Count) - 1;
                var itemEscolhido = inventario[escolha];

                var poolAlvos = itemEscolhido.AlvoEhInimigo ? timeInimigo : timeJogador;
                var vivos = poolAlvos.Where(p => p.EstaVivo).ToList();
                Console.WriteLine(itemEscolhido.AlvoEhInimigo ? "Usar em qual inimigo?" : "Usar em quem?");
                for (int i = 0; i < vivos.Count; i++)
                    Console.WriteLine($"{i + 1} - {vivos[i].Nome} ({vivos[i].Vida}/{vivos[i].VidaMaxima} HP)");
                int alvoIdx = LerOpcaoEntre(1, vivos.Count) - 1;

                itemEscolhido.Usar(vivos[alvoIdx]);
                inventario.RemoveAt(escolha);
                return false;
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

                var alvo = inimigosVivos[alvoIdx];
                alvo.ReceberDano(resultado.Valor);
                if (resultado.AplicaDebuff)
                    alvo.AplicarEfeitoDeAtaque("Fraqueza", -4, 2);
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

            return false;
        }

        static void TurnoInimigo(Personagem atual, List<Personagem> timeInimigo, List<Personagem> timeJogador)
        {
            var resultado = atual.Agir();
            Console.WriteLine($"\n{resultado.Descricao}");

            if (resultado.Tipo == TipoAcao.Ataque)
            {
                if (resultado.Valor > 0)
                {
                    var alvos = timeJogador.Where(p => p.EstaVivo).ToList();
                    var alvo = alvos[rng.Next(alvos.Count)];
                    alvo.ReceberDano(resultado.Valor);
                    if (resultado.AplicaDebuff)
                        alvo.AplicarEfeitoDeAtaque("Fraqueza", -4, 2);
                }
            }
            else
            {
                timeInimigo.Where(p => p.EstaVivo).OrderBy(p => p.Vida).First().Curar(resultado.Valor);
            }
        }

        static void Loja(List<Item> inventario, ref int ouro)
        {
            var catalogo = new List<(string descricao, int preco, Func<Item> criar)>
            {
                ("Poção de Cura (25 HP)", 15, () => new PocaoCura(25)),
                ("Poção de Força (+5 ataque, 3 turnos)", 20, () => new PocaoForca(5, 3)),
                ("Frasco de Veneno (6 dano/turno, 3 turnos)", 25, () => new FrascoVeneno(6, 3)),
            };

            while (true)
            {
                Console.WriteLine($"\n===== LOJA (Ouro: {ouro}) =====");
                for (int i = 0; i < catalogo.Count; i++)
                    Console.WriteLine($"{i + 1} - {catalogo[i].descricao} — {catalogo[i].preco} ouro");
                Console.WriteLine("0 - Sair da loja");

                int escolha = LerOpcaoEntre(0, catalogo.Count);
                if (escolha == 0) break;

                var item = catalogo[escolha - 1];
                if (ouro < item.preco)
                {
                    Console.WriteLine("Ouro insuficiente!");
                    continue;
                }

                ouro -= item.preco;
                inventario.Add(item.criar());
                Console.WriteLine("Comprado!");
            }
        }

        static void SalvarJogo(List<Personagem> time, List<Item> inventario, int ouro, int onda)
        {
            var dados = new JogoSalvo
            {
                Ouro = ouro,
                Onda = onda,
                Time = time.Select(p => new PersonagemSalvo { Tipo = p.GetType().Name, Nome = p.Nome, Vida = p.Vida }).ToList(),
                Inventario = inventario.Select(i => i.Nome).ToList(),
            };

            string json = JsonSerializer.Serialize(dados, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ArquivoSave, json);
            Console.WriteLine($"Jogo salvo em {ArquivoSave}!");
        }

        static void CarregarJogo(out List<Personagem> time, out List<Item> inventario, out int ouro, out int onda)
        {
            string json = File.ReadAllText(ArquivoSave);
            var dados = JsonSerializer.Deserialize<JogoSalvo>(json);

            time = dados.Time.Select(p => CriarPersonagemComVida(p.Tipo, p.Nome, p.Vida)).ToList();
            inventario = dados.Inventario.Select(RecriarItemPorNome).ToList();
            ouro = dados.Ouro;
            onda = dados.Onda;

            Console.WriteLine($"Jogo carregado! Onda {onda}, Ouro: {ouro}");
        }

        static Personagem CriarPersonagemComVida(string tipo, string nome, int vidaSalva)
        {
            Personagem p = CriarPersonagemPorTipo(tipo, nome);
            int diferenca = p.Vida - vidaSalva;
            if (diferenca > 0) p.ReceberDano(diferenca);
            return p;
        }

        static Item RecriarItemPorNome(string nome)
        {
            return nome switch
            {
                "Poção de Cura" => new PocaoCura(25),
                "Poção de Força" => new PocaoForca(5, 3),
                "Frasco de Veneno" => new FrascoVeneno(6, 3),
                _ => new PocaoCura(25),
            };
        }

        static Personagem EscolherPersonagem(string nomeSlot)
        {
            Console.WriteLine($"\n{nomeSlot}:");
            Console.WriteLine("1-Guerreiro 2-Mago 3-Arqueiro 4-Curandeiro 5-Ladrão");
            int tipo = LerOpcaoEntre(1, 5);
            Console.Write("Nome do personagem: ");
            string nome = Console.ReadLine();
            return CriarPersonagem(tipo, nome);
        }

        static Personagem CriarPersonagem(int tipo, string nome)
        {
            return tipo switch
            {
                1 => new Guerreiro(nome),
                2 => new Mago(nome),
                3 => new Arqueiro(nome),
                4 => new Curandeiro(nome),
                5 => new Ladrao(nome),
                _ => throw new ArgumentException("Tipo inválido."),
            };
        }

        static Personagem CriarPersonagemPorTipo(string tipo, string nome)
        {
            return tipo switch
            {
                "Guerreiro" => new Guerreiro(nome),
                "Mago" => new Mago(nome),
                "Arqueiro" => new Arqueiro(nome),
                "Curandeiro" => new Curandeiro(nome),
                "Ladrao" => new Ladrao(nome),
                "Chefe" => new Chefe(nome),
                _ => throw new ArgumentException("Tipo desconhecido: " + tipo),
            };
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