using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

/*
 * Try Catch para tratamento de erros
 *      - Exceções não checadas (não obrigatorio o tratamento
 */

namespace Fundamentos.TryCatch{
        public class Conta{
                private double _saldo;

                public Conta(double saldo){
                        _saldo = saldo;
                }

                public void sacar(double valor){
                        if (valor > _saldo){
                                throw new ArgumentException("Saldo insuficiente.");
                        }

                        _saldo -= valor;
                }
        }

        // Criando exception

        public class NegativoException : Exception{
                public NegativoException(){ }
                public NegativoException(string message) : base(message){ }
                public NegativoException(string message, Exception inner) : base(message, inner){ }
        }

        public class imparException : Exception{
                public imparException(string mensagem) : base(mensagem){ }
        }


        class Tratamentos{
                public static int PositivoPar(){
                        Random random = new Random();
                        int valor = random.Next(-30, 30);

                        if (valor < 0){
                                throw new NegativoException("Numero negativo... :(");
                        }

                        if (valor % 2 == 1){
                                throw new NegativoException("Valor Impar :(");
                        }

                        return valor;
                }


                public static void Executar(){
                        var conta = new Conta(1_223.45);

                        try{
                                conta.sacar(1600);
                                Console.WriteLine("Retirada com sucesso!");
                        } catch (Exception e){
                                Console.WriteLine(e.Message);
                        } finally{
                                Console.WriteLine("Obrigado!");
                        }

                        try{
                                Console.WriteLine(PositivoPar());
                        } catch (NegativoException ex){
                                Console.WriteLine(ex.Message);
                        } catch (imparException ex){
                                Console.WriteLine(ex.Message);
                        }
                }
        }
}