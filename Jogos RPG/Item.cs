using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Item.cs
namespace JogoRPG
{
    internal abstract class Item
    {
        public string Nome { get; set; }

        protected Item(string nome)
        {
            Nome = nome;
        }

        public abstract void Usar(Personagem alvo);
    }
}