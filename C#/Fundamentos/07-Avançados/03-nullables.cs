using System;
using System.Collections.Generic;
using System.Text;


namespace Fundamentos.Avançados {
    class Nullables {
        public static void Executar() {
            // Formas de declarar:

            Nullable<int> num1 = null;
            int? num2 = null;

            if (num1.HasValue) {
                Console.WriteLine($"Valor de num: {num1}");
            } else {
                Console.WriteLine("A variavel não possui valor.");
            }

            int valor = num1 ?? 1000;
            Console.WriteLine(valor);

            bool? booleana = null;
            bool resultado = booleana.GetValueOrDefault();
            Console.WriteLine(resultado);




        }

    }
}
