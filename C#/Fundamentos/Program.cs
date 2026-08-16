using Fundamentos.api;
using Fundamentos.Basicos;
using Fundamentos.Colecoes;
using Fundamentos.Funcoes;
using Fundamentos.POO;
using Fundamentos.TryCatch;
using Fundamentos.Avançados;
using System;
using System.Collections.Generic;


namespace Fundamentos{
        class Program{
                static void Main(string[] args){
                        var central = new CentralDeExercicios(new Dictionary<string, Action>(){
                                // Basicos
                                { "Variaveis e output - Fundamentos", Variaveis.Executar },
                                { "Manipulação de dados - Fundamentos", Manipulacao.Executar },
                                { "Operadores - Fundamentos", Operadores.Executar },
                                { "Unarios e Ternarios - Fundamentos", UnariosTernarios.Executar },
                                { "Estruturas de Controle - Fundamentos", EstruturasDeControle.Executar },
                                { "Enum e struct", Estruturas.Executar },
                                // Coleções
                                { "Arrays", Arrays.Executar },
                                { "Listas", Listas.Executar },
                                { "ArrayList", ColecaoArrayList.Executar },
                                { "Filas", ColecaoArrayList.Executar },
                                { "Igualdade", ColecaoArrayList.Executar },
                                // Classes e metodos
                                { "Classes", Poo.Executar },
                                { "Interface", Interfaces.Executar },
                                // Funcoes Lambda
                                { "Lambda", Lambda.Executar },
                                { "Extenção", Extencao.Executar },
                                // tratamentos de Erros
                                { "Tratamento de Erros", Tratamentos.Executar },
                                // API do C#
                                { "Escrevendo dados", EscrevendoDados.Executar },
                                { "Lendo dados", LendoArquivos.Executar },
                                { "Diretorios", diretorios.Executar },
                                { "caminhos", caminho.Executar },
                                // Avançados
                                { "Linq", Linq.Executar },
                                { "Linq - 02", Linq2.Executar },
                                { "Nullables", Nullables.Executar },
                                { "Dynamics", Dynamics.Executar },
                                { "Generics", Generics.Executar },

                        });

                        central.SelecionarEExecutar();
                }
        }
}