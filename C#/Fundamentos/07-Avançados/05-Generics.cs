using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Avançados {

    public class Caixa<T> {
        T valorPrivado;
        public T Coisa { get; set; };

        public Caixa(T coisa) {
            coisa = coisa;
            valorPrivado = coisa;
        }

        public T metodoGenerico(T valor) {
            return new Random().Next(0, 2) == 0 ? Coisa : valor;
        }

        public T GetValor() {
            return valorPrivado;
        }
    }

    class CaixaInt: Caixa<int> {
        public CaixaInt(): base(0) {

        }
    }
    class Generics {
        public static void Executar() {
            var caixa1 = new Caixa<int>(1000);

            Console.WriteLine(caixa1.metodoGenerico(33));
            Console.WriteLine(caixa1.Coisa.GetType());

            var caixa2 = new Caixa<string>("Construtor");
                Console.WriteLine(caixa2.metodoGenerico("Metodo"));
            Console.WriteLine(caixa2.Coisa.GetType());
        }
    }
}
