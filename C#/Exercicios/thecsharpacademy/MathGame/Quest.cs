using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MathGame {
    class Quest {
        public int N1 { get; set; }
        public int N2 { get; set; }
        public int Result { get; set; }
        public Operator Op { get; set; }
        public Difficulty difficulty { get; set; }

        public void SetDifficulty() {
            bool isValid = false;
            do {
                Console.WriteLine("select your challenger:");
                Console.WriteLine($"[0] - {Difficulty.Random}");
                Console.WriteLine($"[1] - {Difficulty.Easy}");
                Console.WriteLine($"[2] - {Difficulty.Normal}");
                Console.WriteLine($"[3] - {Difficulty.Hard}");

                Enum.TryParse(Console.ReadLine(), out Difficulty parseDifficulty);

                if(Enum.IsDefined(typeof(Difficulty), parseDifficulty)) {
                    difficulty = parseDifficulty;
                    isValid = true;
                } else {
                    Console.WriteLine("Choose a valid option!");
                }

            } while(!isValid);
        }

        public void SetOp() {
            bool isValid = false;
            do {
                Console.WriteLine("What kind of challenger:");
                Console.WriteLine($"[0] - {Operator.Addition}");
                Console.WriteLine($"[1] - {Operator.Subtraction}");
                Console.WriteLine($"[2] - {Operator.Multiplication}");
                Console.WriteLine($"[3] - {Operator.Division}");

                Enum.TryParse(Console.ReadLine(), out Operator parseOperator);

                if(Enum.IsDefined(typeof(Operator), parseOperator)) {
                    Op = parseOperator;
                    isValid = true;
                } else {
                    Console.WriteLine("Is a valid option!");
                }

            } while(!isValid);
        }

        public void SetNumbers(Difficulty d) {
            int min = 0;
            int max = 0;
            if(d == Difficulty.Easy) {
                min = 1;
                max = 11;
            }
            if(d == Difficulty.Normal) {
                min = 2;
                max = 21;
            }
            if(d == Difficulty.Hard) {
                min = 11;
                max = 100;
            }

            N1 = Utils.GenNumber(min, max);
            N2 = Utils.GenNumber(min, max);
        }

    }
}
