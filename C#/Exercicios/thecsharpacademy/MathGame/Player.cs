using System;
using System.Collections.Generic;
using System.Text;

namespace MathGame {
    class Player {
        public string? Name { get; set; }
        public int Score { get; set; }
        public int Answer { get; set; }

        public void SetName() {
            Console.Write("What is your name: ");
            bool validName = false;
            while(!validName) {
                this.Name = Console.ReadLine();
                if(this.Name != null && this.Name.Length > 3) {
                    validName = true;
                } else {
                    Console.Write("Please enter a valid name: ");
                }
            }
        }

        public void UpdateScore(bool isCorrect) {
            if(isCorrect) {
                Console.WriteLine("Correct!");
                Score++;
            } else {
                Console.WriteLine("Incorrect!");
                Score--;
            }
        }

    }
}
