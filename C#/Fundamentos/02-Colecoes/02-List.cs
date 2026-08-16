using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Colecoes{

    public class Produto{
        public string Nome;
        public double Preco;

        public Produto(string nome, double preco){
            Nome = nome;
            Preco = preco;
        }

        public override bool Equals(object? obj){
            Produto outro = (Produto)obj;
            bool mesmoNome = Nome == outro.Nome;
            bool mesmoPreco = Preco == outro.Preco;
            return mesmoNome && mesmoPreco;
        }
    }
    class Listas{
        public static void Executar(){
            var livro = new Produto("Livro", 5.0);

            var carrinho = new List<Produto>();

            var combo = new List<Produto>{
                new Produto("Camiseta", 5.0),
                new Produto("Poster", 10.0),
                new Produto("Aleira", 5.0)
            };
            
            carrinho.AddRange(combo);
            foreach (var item in carrinho){
                Console.WriteLine(item.Nome);
            }


        }
    }
}

