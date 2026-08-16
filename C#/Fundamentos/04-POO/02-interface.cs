using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.POO{
    class Interfaces{

        // 100% dos metodos de uma interface são abstratas 
        interface OpBinaria{
            // Metodos por padrão publico
            int Operacao(int a, int b);
        }

        class Soma : OpBinaria{
            public int Operacao(int a, int b){
                return a + b;
            } 
        }

        class Subtracao : OpBinaria{
            public int Operacao(int a, int b){
                return a - b;
            }
        }

        class Multiplicacao : OpBinaria{
            public int Operacao(int a, int b){
                return a * b;
            }
        }

        class calculadora{
            private List<OpBinaria> operacoes = new List<OpBinaria>{
                new Soma(),
                new Subtracao(),
                new Multiplicacao()
            };

            public string ExecutarOperacoes(int a, int b){
                string resultado = "";

                foreach (var op in operacoes){
                    resultado += $"usando {op.GetType().Name} = {op.Operacao(a, b)}";
                }
                
                return resultado;
            }
        }
        
        public static void Executar(){
            var calc = new calculadora();
            var resultado = calc.ExecutarOperacoes(25, 5);
            Console.WriteLine(resultado);
            
        }
    }
}
