using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MathGame {

    public enum Difficulty {
        Random,
        Easy,
        Normal,
        Hard
    }
    public enum Operator {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public static class ExtensionOperators {
        public static string ToSymbol(this Operator op) => op switch {
            Operator.Addition => "+",
            Operator.Subtraction => "-",
            Operator.Multiplication => "*",
            Operator.Division => "/",
            _ => "?"
        };
    }
    class Utils {

        private static Random rnd = new Random();

        public static int GenNumber(int min, int max) {
            return rnd.Next(min, max);
        }


    }
}
