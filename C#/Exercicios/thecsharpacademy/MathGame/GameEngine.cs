using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace MathGame {
    class GameEngine {
        public static void Start() {
            Player player = new Player();
            Quest quest = new Quest();
            History history = new History();
            int menu = 0;
            Console.WriteLine("Welcome to MathGame!");
            Console.WriteLine("Would you like to say your name?");
            player.SetName();
            Console.WriteLine($"Very nice {player.Name}!");

            while(menu != 9) {

                player.Score = 0;

                Console.WriteLine("What would you like to do:");
                Console.WriteLine("[1] - Start Game");
                Console.WriteLine("[2] - History");
                Console.WriteLine("[9] - Exit");
                int.TryParse(Console.ReadLine(), out menu);

                switch(menu) {
                    case 1:
                        quest.SetDifficulty();
                        if(quest.difficulty == Difficulty.Random) {
                            Games.RandomGame(quest, player, history);
                        } else {
                            quest.SetOp();
                            int loop = 0;
                            while(loop < 5) {
                                Games.PlayQuestion(quest, player, history);
                                loop++;
                            }
                        }
                        Console.WriteLine($"Final score: {player.Score}");

                        if(player.Score == 5) {
                            Console.WriteLine("Congratulations!!!");
                            Console.WriteLine($"Your answer for the 5 quests was correct!");
                        }
                        if(player.Score > 2 && player.Score <= 4) {
                            Console.WriteLine("You did good, but can be better!");
                            Console.WriteLine($"Your score {player.Score}.");
                        }
                        if(player.Score <= 1) {
                            Console.WriteLine("Keep trying");
                            Console.WriteLine($"Your score was only {player.Score}");
                        }
                        break;
                    case 2:
                        history.GetHistory();
                        break;
                    case 9:
                        Console.WriteLine("Thank you for playing!");
                        break;
                    default:
                        Console.WriteLine($"{menu} is not a valid option");
                        break;
                }
            }

        }
    }
}

