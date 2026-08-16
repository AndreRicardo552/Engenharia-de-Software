using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// Primeiro a Entrar ultimo a sair, contrario de fila

namespace Fundamentos.Colecoes
{
    class Pilha{
        public static void Executar(){
            var pilha = new Stack();

            pilha.Push(3);
            pilha.Push("A");
            pilha.Push(true);
            
            Console.WriteLine(pilha.Peek());
            
            Console.WriteLine(pilha.Pop());

            foreach (var item in pilha){
                Console.WriteLine(item);
            }
        }
    }
};

