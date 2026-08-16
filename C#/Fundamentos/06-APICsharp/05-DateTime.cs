using System;
using System.Collections.Generic;
using System.Text;

namespace Fundamentos.api {
    class ExDatetime {
        public static void Executar() {
            var dateTime = new DateTime(year: 2030, month: 2, day: 6);

            Console.WriteLine(dateTime.Day);
            Console.WriteLine(dateTime.Month);
            Console.WriteLine(dateTime.Year);

            var hoje = DateTime.Today;
            Console.WriteLine(hoje);

            var diaAtual = DateTime.Now;
            Console.WriteLine(diaAtual);

            /*
                Hã diversos metodos dentro de DateTime
             */

            // TimeSpan - para trabalhar com intervalos de tempo

            var intervalo = new TimeSpan(days: 10, hours: 20, minutes: 30, seconds: 40);
            Console.WriteLine(intervalo);
            Console.WriteLine("Minutos: " + intervalo.Minutes);
            Console.WriteLine("INtervalo em minutos: " + intervalo.TotalMinutes);




        }
    }
}
