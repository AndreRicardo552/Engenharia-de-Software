using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Fundamentos.Avançados {

    class Aluno {
        public string nome;
        public int idade;
        public double nota;
    }
    class Linq {
        public static void Executar() {
            var alunos = new List<Aluno> {
                new Aluno() {nome = "André", idade = 24, nota = 8.0},
                new Aluno() {nome = "Pedro", idade = 28, nota = 4.3},
                new Aluno() {nome = "Joana", idade = 21, nota = 9.5},
                new Aluno() {nome = "Ricardo", idade = 25, nota = 8.5},
                new Aluno() {nome = "Tatiane", idade = 20, nota = 7.7},
                new Aluno() {nome = "Pedro", idade = 22, nota = 7.5},
                new Aluno() {nome = "Elias", idade = 18, nota = 6.8},
            };

            Console.WriteLine("=== Aprovado =========");
            // Where(parecido com o where do sql)
            // para cada elementos na lista de alunos onde "a", retorna
            var aprovados = alunos.Where(a => a.idade > 24); // Função para outra função Lambda dentro de lambda
            foreach(var aluno in aprovados) {
                Console.WriteLine(aluno.nome);
            }
            aprovados = alunos.Where(a => a.idade >= 7).OrderBy(a => a.nome);
            foreach (var aluno in aprovados) {
                Console.WriteLine(aluno.nome);
            }
            // Chamada (map, transformar um array em outro array)
            // OrderBy transforma a lista ordenada por nome e apos isso select pega somente os campos de nome e descarta os outros 
            var chamada = alunos.OrderBy(a => a.nome).Select(a => a.nome); 
            foreach (var aluno in chamada) {
                Console.WriteLine(aluno);
            }

            var alunosAprovados = 
                from aluno in alunos // Para cada aluno na lista de alunos
                where aluno.nota >= 7 // Seleciona somente onde a nota for maior que 7
                orderby aluno.idade // Ordenado pela idade(apenas dos aprovados)
                select aluno.nome; // map(transforma em uma array de strings de nomes dos aprovados)

            foreach (var aluno in alunosAprovados) {
                Console.WriteLine(aluno);
            }

        }
    }
}
