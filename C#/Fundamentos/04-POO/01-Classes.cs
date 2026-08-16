using System;
using System.Diagnostics.Metrics;

/*
 *  Programação orientada a objetos
 *
 *      Os 4 Pilares:
 *          - Abstração
 *          - Heranças
 *          - Encapsulamento
 *          - Polimofirsmo
 *
 *      Metodos e Metodos Estaticos
 *      Construtores
 *      Get - Set
 *      Auto implementado {get; set;} 
 *      Propriedades
 */

/*
 *  Tipos de encapsulamento:
 *      - Protected = Transmitido somente por herança
 *      - Internal = Mesmo Projeto(assembly) 
 *      - Pretected internal = Transmitido por herança ou no mesmo projeto
 *      - private protected = Mesma classe ou herança no mesmo projeto
 *      - private = Somente visivel dentro da propria classe
 * 
 *  Abstração de classes usa-se a palavra abstract
 *      - Pode-se ter metodos abstratos(sem corpo);
 *
 *  Classes e Metodos Sealed
 *      - Proibe Herança
 *      - Proibe override
 */

namespace Fundamentos.POO{

    public class Carro{
        // Atributos:
        protected readonly int VelMax; 
        private int _velAtual; 

        // Construtor
        public Carro(int velMax){
            VelMax = velMax;
        }

        // Metodos
        protected int AltVel(int delta){
            int nVel = _velAtual +  delta;
            if (nVel < 0){
                _velAtual = 0;
            } else if (nVel > VelMax){
                _velAtual = VelMax;
            }else{
                _velAtual = nVel;
            }
            
            return _velAtual;
        }

        public virtual int Acelerar(){ // Virtual permite sobreescrever o metodo
            return AltVel(5);
        }

        public int Frear(){ 
            return AltVel(-5);
        }
    }

    // Herança 
    public class Uno : Carro{
        public Uno() : base(200){}
        // Posso usar public Uno() : this(){} - Para chamar o construtor da propria classe
    }

    public class Ferrari : Carro{
        public Ferrari() : base(400){}

        // Sobreescreve o metodo da classe pai
        public override int Acelerar(){
            return AltVel(15);
        } // Substitui o da classe pai em caso de polimorfismo

        // Oculta o metodo da classe pai
        public new int Frear(){
            return AltVel(-15);
        } // Não substitui o da classe pai em caso de polimorfismo
    }
    
    class Poo{
        public static void Executar(){
            
            
            // Instanciando
            Uno c1 = new Uno(); 
            Ferrari c2 = new Ferrari();
            // Polimorfismo - permite criar apatir de uma abstração(classe pai) um objeto filho
            Carro c3 = new Ferrari();
            
            Console.WriteLine("Uno...");
            Console.WriteLine(c1.Acelerar());
            Console.WriteLine(c1.Acelerar());
            Console.WriteLine(c1.Frear());
            Console.WriteLine(c1.Frear());
            
            Console.WriteLine("Ferrari...");
            Console.WriteLine(c2.Acelerar());
            Console.WriteLine(c2.Acelerar());
            Console.WriteLine(c2.Frear());
            Console.WriteLine(c2.Frear());
            
            Console.WriteLine("Carro 3(ferrari)...");
            Console.WriteLine(c3.Acelerar());
            Console.WriteLine(c3.Acelerar());
            Console.WriteLine(c3.Frear());
            Console.WriteLine(c3.Frear());
            
            c3 = new Uno(); // Polimorfismo usado para nova instancia de um objeto dentro do generico
            Console.WriteLine("Carro 3(Uno)...");
            Console.WriteLine(c2.Acelerar());
            Console.WriteLine(c2.Acelerar());
            Console.WriteLine(c2.Frear());
            Console.WriteLine(c2.Frear());
            
        }
        
    }
    
    
}