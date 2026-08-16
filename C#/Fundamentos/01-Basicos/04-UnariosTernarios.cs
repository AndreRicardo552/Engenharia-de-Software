using System;

// Opera sobre um unico operando para realizar incremento, decremento, negação ou inversão de sinal

namespace Fundamentos.Basicos{
    class UnariosTernarios{
        public static void Executar(){
            var negativo = -5;
            var n1 = 2;
            var n2 = 3;
            var booleano = true;
            
            // Exemplos de unarios
            Console.WriteLine(!booleano);
            Console.WriteLine(-negativo);
            
            // Exemplo de Ternario:
            double nota;
            Console.Write("Escreva um valor para a nota: ");
            nota = double.Parse(Console.ReadLine());
            // O primeiro valor é atribuido se verdadeiro se falso o segundo
            var resultado = nota >= 7.0 ? "Aprovado" :  "Reprovado";
            Console.WriteLine(resultado);


        }
    }
}

