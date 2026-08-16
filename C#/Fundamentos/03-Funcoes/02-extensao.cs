using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Funcoes{

    public static class ExtencoesInteiro{
        // A palavra This define que a função, é uma função de extencao para tipos
        public static int Soma(this int num, int val){
            return num + val;
        }
        public static int Subtracao(this int num, int valor){
            return num - valor;
        }
    }
    class Extencao{
        public static void Executar(){
            int numero = 5;
            
            Console.WriteLine(numero.Soma(3));
            Console.WriteLine(numero.Subtracao(5));
            
            Console.WriteLine(2.Soma(3));
            Console.WriteLine(3.Soma(3));
            
            
        }
    }
}

