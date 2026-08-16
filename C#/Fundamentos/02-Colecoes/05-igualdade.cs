using System;
using System.Collections.Generic;
using System.Text;
using Fundamentos.Colecoes;

namespace Fundamentos.colecoes{
    class Igualdade{
        public static void Executar(){

            var p1 = new Produto("Caneta", 1.89);
            var p2 = new Produto("Caneta", 1.89);
            var p3 = p2;
            
            Console.WriteLine(p1 == p2); // Falso(valida endereço de memoria)
            Console.WriteLine(p3 == p2); // Tem o mesmo endereço de memoria
            // Se torna igual se fizer um override na classe consigo parar de comparar endereço de memoria
            Console.WriteLine(p1.Equals(p2));

        }
    }
};