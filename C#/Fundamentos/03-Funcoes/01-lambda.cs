using System;
using System.Collections.Generic;

// Lambda São funcões anonimas - podem ser armazenadas em variaveis

namespace Fundamentos.Funcoes{
    
    // Delegates - Tipo que armazena uma assinatura de uma função
    delegate double Operacao(double x, double y);
    
    class Lambda{
        
        delegate double Soma( double x, double y);
        delegate void impSoma(double a, double b);
        delegate string StringOperacao(string s);
        
        static double MinhaSoma(double x, double y){
            return x + y;
        }

        static void MeuImpSoma(double a, double b){
            Console.WriteLine(a + b);
        }
        
        // Delegate COmo parametro
        public delegate int Op(int x, int y);

        public static int Somar(int x, int y){
            return x + y;
        }
        
        public static int Calculadora(Op op, int x, int y){
            return op(x,y);
        }
        
        public static void Executar(){
            
            // Action - Função sem retorno
            Action somethingConsole = () => {
                Console.WriteLine("Lambda com C#");
            };
            somethingConsole();
            
            // Func - permite retorno
            // Retorna um int
            Func<int> jogarDado = () => {
                Random randon = new Random();
                return randon.Next(1, 7);
            };
            jogarDado();
            
            // Recebe um int e retornar uma string
            Func<int, string> convertHex = n => n.ToString("X"); // Função reduzida
            convertHex(jogarDado());
            
            Func<int, int, int, string> formatDate = (day, mouth, year) => 
                String.Format("{0:D2}/{1:D2}/{2:D4}", day, mouth, year);
            Console.WriteLine(formatDate(1,1,2019));

            // Tipo já foi definido pelo delegate
            Operacao sum = (x, y) => x + y;
            Operacao sub = (x, y) => x - y;
            Operacao mult = (x, y) => x * y;

            // Armazenando função já existente em um delegate
            // - desde que haja compatibilidade
            Soma op1 = MinhaSoma;
            Console.WriteLine(op1(2,3));
            
            impSoma op2 = MeuImpSoma;
            op2(2.2,3.9);
            
            // Delegate como Funcões anonimas
            StringOperacao inverter = delegate(string s){
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            };
            Console.WriteLine(inverter("André"));
            
            
            

        }
    }
}

