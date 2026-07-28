using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JogoRPG
{
    internal class Curandeiro : Personagem
    {
        private static Random rng = new Random();
        private int poderCura = 18;

        public Curandeiro(string nome) : base(nome, vidaMaxima: 70, ataqueBase: 6) { }

        public override ResultadoAcao Agir()
        {
            bool curaMaior = rng.Next(0, 100) < 20;
            int cura = curaMaior ? poderCura * 2 : poderCura;
            string desc = curaMaior
                ? $"{Nome} canaliza uma CURA INTENSA! (+{cura} HP)"
                : $"{Nome} recita uma prece de cura. (+{cura} HP)";

            return new ResultadoAcao { Tipo = TipoAcao.Cura, Valor = cura, Descricao = desc };
        }
    }
}