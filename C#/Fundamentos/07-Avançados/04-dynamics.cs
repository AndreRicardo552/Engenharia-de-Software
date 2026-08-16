using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Avançados {
    class Dynamics {
        public static void Executar() {
            dynamic meuObjeto = "teste";
            meuObjeto = 3;

            meuObjeto++;
            Console.WriteLine(meuObjeto);
        }
    }
}
