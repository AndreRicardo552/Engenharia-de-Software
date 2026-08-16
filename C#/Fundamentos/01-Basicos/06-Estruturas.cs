using System;
using System.Collections.Generic;

// Enum (enumeração)


namespace Fundamentos.Basicos{

    // Exemplo de Enum
    public enum Genero{
        Acao,
        Aventura,
        Terror,
        Animacao,
        Comedia
    };
    
    public class Filme{
        public string Titulo;
        public Genero Genero;
    }

    //Exemplo de struct
    interface IPonto{
        void MoverNaDiagonal(int delta);
    }

    struct Coordenada : IPonto{
        public int x;
        public int y;

        public Coordenada(int x, int y){
            this.x = x;
            this.y = y;
        }

        public void MoverNaDiagonal(int delta){
            x += delta;
            y += delta;
        }
    }

    
    public class Estruturas{
        public static void Executar(){
            int id = (int)Genero.Animacao;
            Console.WriteLine(id);

            var novoFilme = new Filme();
            novoFilme.Titulo = "Shaknado";
            novoFilme.Genero = Genero.Comedia;
            
            Console.WriteLine(novoFilme);
            
            
            Coordenada coordenada = new Coordenada();
            coordenada.x = 10;
            coordenada.y = 10;
            Console.WriteLine("Cordenada inical: " + coordenada);
            
            var coordenadaFinal =  new Coordenada(x: 9, y: 1);
            coordenadaFinal.MoverNaDiagonal(10);
            Console.WriteLine(coordenadaFinal);

        }
    }
}

