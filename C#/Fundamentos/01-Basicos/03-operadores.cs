using System;

// Operadores aritimeticos, logicos e relacionais

namespace Fundamentos.Basicos{
    class Operadores{
        public static void Executar(){
            // Operadores - Padrões de todas as linguagens de programação
            /*
             * Aritimeticos:
             * + Soma
             * - Subtração
             * * Multiplicacão
             * / Divisão
             * % Modulo
             * ++ -- incremento/decremento
             *
             * Logicos(consultar tabela Verdade em duvida):
             * || OR
             * && AND
             * !  NOT
             *
             * Relacionais:
             * > Maior que
             * < Menor que
             * == Igual
             * >= Maior ou igual
             * <= Menor ou igual
             *
             * Atribuição:
             * = Atribuivalor
             * += Soma e atribui
             * -= Subtrai e atribui
             * *= Multiplica e atribui
             * /= Divide e atribui
             *  Existe diversos outros tipos de atribuição
             */

            double altura, peso, imc;

            Console.Write("Digite o valor do altura: ");
            altura = double.Parse(Console.ReadLine());
            Console.Write("Digite o valor do peso: ");
            peso = double.Parse(Console.ReadLine());

            imc = peso / (altura * altura);

            Console.WriteLine($"Seu IMC é {imc}");
            
            
        }
    }
};