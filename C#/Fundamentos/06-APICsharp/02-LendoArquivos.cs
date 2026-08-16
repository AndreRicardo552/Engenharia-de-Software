using System;
using System.IO;

namespace Fundamentos.api {

    class LendoArquivos {
        public static void Executar() {
            // para linux:
            // var path = @"~/DEV/Estudo/Linguagens/C#/Fundamentos/primeiro_arquivo.txt".ParseHome();
            // Para windows:
            var path = @"C:/DEV/Estudo/Linguagens/C#/Fundamentos/Leitura_Arquivo.txt".ParseHome();

            if (!File.Exists(path)) {
                using (StreamWriter sw = File.AppendText(path)) {
                    sw.WriteLine("Escrevendo novamente");
                    sw.WriteLine("Escrevendo novamente");
                    sw.WriteLine("Escrevendo novamente");
                }
            }

            try {
                using (StreamReader sr = new StreamReader(path)) {
                    var texto = sr.ReadToEnd();
                    Console.WriteLine(texto);
                } 
            } catch( Exception er){
                Console.WriteLine(er.Message);
            };




        }
    }
}

