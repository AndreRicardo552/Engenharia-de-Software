using System;

/*  Estruturas:
 *      if/else
 *      switch
 *      While/DoWhile
 *      For
 *      ForEach
 *
 *  Utilização do break e do continue
 */

namespace Fundamentos.Basicos{
    class EstruturasDeControle{
        public static void Executar(){
            int n1 = 5;

            //if, else if e else
            if (n1 % 2 != 0){
                Console.WriteLine("O numero não é par");
            }
            else if (n1 % 2 == 0){
                Console.WriteLine("O numero é par");
            }
            else{
                Console.WriteLine("não foi possivel determinar se é par ou impar");
            }

            var menu = 2;

            // Switch-case
            switch (menu){
                case 1:{
                    Console.WriteLine("Caso 1");
                    break;
                }
                case 2:{
                    Console.WriteLine("Caso 2");
                    break;
                }
            }

            // While-DoWhile
            do{
                n1++;
            } while (n1 < 10);

            //for
            for (int i = 0; i < n1; i++){
                Console.WriteLine($"Contando: {i}");
            }

            // Foreach - Anda do começo ao fim
            string[] carros = { "Volvo", "BMW", "Ford", "Mazda" };
            foreach (string i in carros){
                Console.WriteLine(i);
            }
        }
    }
}