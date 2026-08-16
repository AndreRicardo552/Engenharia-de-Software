using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Colecoes{
    class Filas{
        public static void Executar(){
            //Declaração
            var fila = new Queue<string>();
            // Adicionando
            fila.Enqueue("Maria");
            fila.Enqueue("André");
            fila.Enqueue("Ricardo");
            // Ve o primeiro da fila sem remover
            Console.WriteLine(fila.Peek());
            Console.WriteLine(fila.Count());
            // Remove o primeiro
            Console.WriteLine(fila.Dequeue());
            Console.WriteLine(fila.Count());

            foreach (var item in fila){
                Console.WriteLine(item);
            }

            // Fila que aceita valores genericos
            var salada = new Queue();
            salada.Enqueue("Maria");
            salada.Enqueue(3);
            salada.Enqueue(3.14);
            salada.Enqueue(true);
            
            // Verifica se existe na fila
            Console.WriteLine(salada.Contains("Maria"));

        }
    }
}

