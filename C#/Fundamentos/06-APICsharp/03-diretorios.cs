using Fundamentos.api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fundamentos.api {
    class diretorios {
        public static void Executar() {
            var novoDir = @"C:/DEV/Estudo/Linguagens/C#/Fundamentos/novoDir".ParseHome();
            var novoDirDestino = @"C:/DEV/Estudo/Linguagens/C#/Fundamentos/novoDirDestino".ParseHome();
            var dirProjeto = @"C:/DEV/Estudo/Linguagens/C#/Fundamentos/".ParseHome();

            if (Directory.Exists(novoDir)) {
                Directory.Delete(novoDir, true);
            }

            if (Directory.Exists(novoDirDestino)) {
                Directory.Delete(novoDirDestino, true);
            }

            Directory.CreateDirectory(novoDir); // Cria um diretorio
            Console.WriteLine(Directory.GetCreationTime(novoDir));

            Console.WriteLine("== pastas =============");
            var pastas = Directory.GetDirectories(dirProjeto); // Pega os diretorios dentro do diretorio
            foreach(var pasta in pastas) {
                Console.WriteLine(pasta);
            }

            Console.WriteLine("== arquivos ==========");
            var arquivos = Directory.GetFiles(dirProjeto); // pega os arquivos do diretorio
            foreach(var arquivo in arquivos) {
                Console.WriteLine(arquivo);
            }

            Console.WriteLine(Directory.GetDirectoryRoot(novoDir)); // Pega a raiz do diretorio

            Directory.Move(novoDir, novoDirDestino); // Move o diretorio

            // dirInfo
            // metodos parecidios com o Directory
            var dirInfo = new DirectoryInfo(dirProjeto);
            
            if (!dirInfo.Exists) {
                dirInfo.Create();
            }

            
            



        }
    }
}
