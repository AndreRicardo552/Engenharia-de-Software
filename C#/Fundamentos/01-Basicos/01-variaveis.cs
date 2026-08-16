using System;

// Variaveis em C#

namespace Fundamentos.Basicos{
    class Variaveis{
        public static void Executar(){
            // Declaração e inicialização de variaveis

            // Inteiros
            // Se eu declarar unsiged usa-se u = uint, ulong
            byte tipoByte = byte.MaxValue; // 1 byte(8bits) sem sinal
            sbyte tipoSByte = SByte.MaxValue; // 1 byte(8bits) com sinal
            short tipoShort = short.MaxValue; // 2 bytes(16bits) com sinal
            int tipoInteiro = int.MaxValue; // 4 byte (32bits) com sinal
            long tipoLong = long.MaxValue; // 8bytes (64bits) com sinal - Deve conter L no final do valor

            Console.WriteLine("Imprimindo tipo Byte e seu valor maximo " + tipoByte);
            Console.WriteLine("Imprimindo tipo sByte e seu valor maximo " + tipoSByte);
            Console.WriteLine("Imprimindo tipo short e seu valor maximo " + tipoShort);
            Console.WriteLine("Imprimindo tipo int e seu valor maximo " + tipoInteiro);
            Console.WriteLine("Imprimindo tipo long e seu valor maximo " + tipoLong);

            // Flutuantes
            float tipoFloat = float.MaxValue; // 4 Bytes - Deve conter F no final do valor
            double tipoDouble = double.MaxValue; // 8 Bytes - Deve conter D no final do valor
            decimal tipoDecimal = decimal.MaxValue; // 16 Bytes(128bits) astronomico
            Console.WriteLine("Imprimindo tipo float e seu valor maximo " + tipoFloat);
            Console.WriteLine("Imprimindo tipo double e seu valor maximo " + tipoDouble);
            Console.WriteLine("Imprimindo tipo decima e seu valor maximo " + tipoDecimal);

            // char e strings
            char tipoChar = 'a'; // 2 Bytes
            string tipoString = "André"; // 2 Bytes por caracteres 
            Console.WriteLine("Imprimindo a string nome: " + tipoString + "Imprimindo um char: " + tipoChar);

            // Booleanos
            bool tipoBooleano = true;
            Console.WriteLine("Imprimindo tipo Bool: " + tipoBooleano);

            // Declaração de constante e inferencia de tipos
            const double pi = 3.14D; // Constante
            var inferencia =
                "Compilador detecta o tipo quando fazemos inferencia usando var"; // Deve ser inicializado na declaração

            Console.WriteLine(inferencia);
            
            // Arrays
            int[] lista;
            string[] carros;
            // matriz
            int[,] tabela;
            string[,] objetos;

            // Type casting - convertendo tipos
            // Cast automatico - tipos menores em tipos maiores
            int x = 10;
            double y = x;
            // Cast Manual - Tipos maiores em tipos menores
            double a = 50;
            int b = (int)a;
            // Parse e TryParse
            string texto = "10";
            int numero;
            // numero = int.Parse(texto); // Faz o parse(se não conseguir perde dados)
            int.TryParse(texto, out numero); // Tenta fazer o parse de string para numero


            // Convertendo tipos usando classe Convert
            Console.WriteLine(Convert.ToString(a));
        }
    }
}