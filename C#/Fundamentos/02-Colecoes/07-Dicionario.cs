using System;
using System.Collections;
using System.Collections.Generic;


namespace  Fundamentos.Colecoes
{
    class ProgramDicionarios{
        public static void Executar(){
            // Dicionarios precisam de uma chave e valor
            var filmes = new Dictionary<int, string>();
            
            // Adiciona a chave e o valor
            filmes.Add(2000, "Gladiador");
            filmes.Add(2002, "Vingadores");
            filmes.Add(2004, "Senhor dos aneis");
            filmes.Add(2006, "O grande truque");
            
            // Ve se a chave esta contida
            if (filmes.ContainsKey(2004)){
                // Da erro se a chave não existir
                Console.WriteLine("2004: " + filmes[2004]);
                // Pega o valor ou um valor default evitando erro no programa
                Console.WriteLine("2004: " + filmes.GetValueOrDefault(2004));
            }
            
            Console.WriteLine(filmes.ContainsValue("Amnésia"));
            // Remove um item
            Console.WriteLine($"Removeu? {filmes.Remove(2004)}");

            // Salvar em uma variavel
            filmes.TryGetValue(2016, out string filme2006);
            Console.WriteLine(filme2006);
            
            // Formas de percorrer um Dicionario
            // Percorre somente chave
            foreach (var ch in filmes.Keys){
                Console.WriteLine(ch);
            }
            // Percorre somente valor
            foreach (var vl in filmes.Values){
                Console.WriteLine(vl);
            }
            // Forma mais complicada percorre chave e valor
            foreach (KeyValuePair<int, string> kvp in filmes){
                Console.WriteLine($"{kvp.Key} - {kvp.Value}");
            }
            // Forma simples percorre chave e valor
            foreach (var filme in filmes){
                Console.WriteLine($"{filme.Key} - {filme.Value}");
            }



        }
    }
};

