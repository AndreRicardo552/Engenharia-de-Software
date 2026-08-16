using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MathGame {
    class Games {

        public static void RandomGame(Quest quest, Player player, History history) {
            int loop = 0;
            while(loop < 5) {
                Difficulty d = (Difficulty)Utils.GenNumber(1, 4);
                Operator op = (Operator)Utils.GenNumber(0, 4);
                quest.difficulty = d;
                quest.Op = op;
                PlayQuestion(quest, player, history);
                loop++;
            }
            quest.difficulty = Difficulty.Random;
        }

        public static void PlayQuestion(Quest quest, Player player, History history) {
            quest.SetNumbers(quest.difficulty);
            if(quest.Op == Operator.Division) {
                while(quest.N1 % quest.N2 != 0) {
                    quest.SetNumbers(quest.difficulty);
                }
            }

            quest.Result = quest.Op switch {
                Operator.Addition => quest.N1 + quest.N2,
                Operator.Subtraction => quest.N1 - quest.N2,
                Operator.Multiplication => quest.N1 * quest.N2,
                Operator.Division => quest.N1 / quest.N2,
                _ => throw new InvalidOperationException("Invalid operator")
            };

            Stopwatch gameTimer = new Stopwatch();
            gameTimer.Start();

            PrintQuest(quest.N1, quest.N2, quest.Op);
            Console.WriteLine("Your answer: ");
            int n;
            while(int.TryParse(Console.ReadLine(), out n) == false) {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
            player.Answer = n;

            gameTimer.Stop();
            double responseTime = gameTimer.Elapsed.TotalSeconds;

            player.UpdateScore(player.Answer == quest.Result);
            history.AddHistory(player, quest, responseTime);

        }

        private static void PrintQuest(int n1, int n2, Operator op) {
            Console.WriteLine($"Whats is the answer of the {op.ToString()}: {n1} {op.ToSymbol()} {n2}");
        }


    }
}
