using System;
using System.IO;

namespace Fundamentos.api{
        public static class ExtensaoString{
                public static string ParseHome(this string path){
                        string home = (Environment.OSVersion.Platform == PlatformID.Unix
                                       | Environment.OSVersion.Platform == PlatformID.MacOSX)
                                ? Environment.GetEnvironmentVariable("HOME")
                                : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

                        return path.Replace("~", home);
                }
        }

        class EscrevendoDados{
                public static void Executar(){
            // para linux:
            // var path = @"~/DEV/Estudo/Linguagens/C#/Fundamentos/primeiro_arquivo.txt".ParseHome();
            // Para windows:
            var path = @"C:/DEV/Estudo/Linguagens/C#/Fundamentos/primeiro_arquivo.txt".ParseHome();


            // Criando na pasta home
            if (!File.Exists(path)){
                                using (StreamWriter sw = File.CreateText(path)){
                                        sw.WriteLine("Esse é");
                                        sw.WriteLine("o nosso");
                                        sw.WriteLine("primeiro");
                                        sw.WriteLine("arquivo");
                                }
                        }
                        // Editando existente
                        using (StreamWriter sw = File.AppendText(path)){
                                sw.WriteLine("Adicionando mais texto");
                        }
                }
        }
}