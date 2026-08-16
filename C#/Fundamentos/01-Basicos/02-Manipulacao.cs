using System;
using System.Globalization;

// Manipulação de dados inclusive inputs

namespace Fundamentos.Basicos{
    class Manipulacao{
        public static void Executar(){
            string nome;
            string sobreNome;
            int idade;
            double salario;

            // Lendo dados do teclado - por padrão é string, deve ser convertido quando numeros
            Console.Write("Qual seu nome: ");
            nome = Console.ReadLine();
            Console.Write("Qual seu sobrenome: ");
            sobreNome = Console.ReadLine();
            Console.Write("Qual sua idade: ");
            idade = int.Parse(Console.ReadLine()); // Posso usar metodo parse ou a classe Convert.ToInt32
            Console.Write("Qual seu salario: ");
            salario = double.Parse(Console.ReadLine(),
                CultureInfo.InvariantCulture); // Usado para não pegaro padrão do sistema

            // Formas de interpolação de string 
            // Console.WriteLine("Nome completo: {0} {1} idade: {2} ", nome, sobreNome, idade);
            Console.WriteLine($"Nome completo: {nome} {sobreNome} tem {idade} anos e ganha R${salario}");

            // Manipulando numeros:
            /*
             * Consigo manipular numeros dentro da impressão usando metodos de conversão como o ToString Ex:
             *
             * Console.WriteLine(salario.ToString("C"); C = Currency(moeda);
             * Console.WriteLine(salario.ToString("F1"); F1 = deixa com apenas 1 casa decimal
             * Console.WriteLine(salario.ToString("P"); P = porcentual % (multiplica o valor por 100)
             * Console.WriteLine(salario.ToString("#.##"); Forma de discrimar o limite do ponto flutuante
             * Console.WriteLine(salario.ToString("D10"); Adiciona 0 até que o numero tenha 10 posições
             */
        }
    }
}