using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.Avançados{
    class Linq2 {
        public static void Executar() {

            var alunos = new List<Aluno> {
                new Aluno() {nome = "André", idade = 24, nota = 8.0},
                new Aluno() {nome = "Pedro", idade = 28, nota = 4.3},
                new Aluno() {nome = "Joana", idade = 21, nota = 9.5},
                new Aluno() {nome = "Ricardo", idade = 25, nota = 8.5},
                new Aluno() {nome = "Tatiane", idade = 20, nota = 7.7},
                new Aluno() {nome = "Pedro", idade = 22, nota = 7.5},
                new Aluno() {nome = "Ana", idade = 18, nota = 6.8},
            };

            var pedro = alunos.Single(aluno => aluno.nome.Equals("Pedro"));
            Console.WriteLine($"{pedro.nome} {pedro.nota}");

            var fulano = alunos.SingleOrDefault(alunoe => alunoe.nome.Equals("Fulano");
            if(fulano == null) {
                Console.WriteLine("Aluno inexistente");
            }

            var ana = alunos.First(aluno => aluno.nome.Equals("Ana")); //existe tbm fistordefault
            Console.WriteLine(ana.nome);

            var ana2 = alunos.LastOrDefault(aluno => aluno.nome.Equals("Ana"));
            Console.WriteLine(ana2.nota);

            var exemploSkip = alunos.Skip(1).Take(3);
            foreach(var item in exemploSkip) {
                Console.WriteLine(item.nome);
            }

            var maiorNota = alunos.Max(aluno => aluno.nota);
            Console.WriteLine(maiorNota);
            var menorNota = alunos.Min(aluno => aluno.nota);
            Console.WriteLine(menorNota);
            var somatorioNotas = alunos.Sum(aluno => aluno.nota);
            Console.WriteLine(somatorioNotas);

            var media = alunos.Average(aluno => aluno.nota);
            Console.WriteLine(media);




        }
    }
}
