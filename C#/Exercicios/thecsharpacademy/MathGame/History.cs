using System;
using System.Collections.Generic;
using System.Text;

namespace MathGame {
    class History {
        private record CompletedQuest(string playerName, int playerAnswer, int playerScore, int correctAnswer, string question, double responseTime);
        private List<CompletedQuest> _cqHistory = new List<CompletedQuest>();

        public void AddHistory(Player player, Quest quest, double responseTime) {
            _cqHistory.Add(new CompletedQuest(player.Name, player.Answer, player.Score, quest.Result, $"{quest.N1} {quest.Op.ToSymbol()} {quest.N2}", responseTime));
        }

        public void GetHistory() {
            foreach(var cqHistory in _cqHistory) {
                Console.WriteLine("===== ===== ===== =====");
                Console.WriteLine($"Player: {cqHistory.playerName}");
                Console.WriteLine($"Question: {cqHistory.question}");
                Console.WriteLine($"Player Answer: {cqHistory.playerAnswer}");
                Console.WriteLine($"Correct Answer: {cqHistory.correctAnswer}");
                Console.WriteLine($"Player Score: {cqHistory.playerScore}");
                Console.WriteLine($"Response Time: {cqHistory.responseTime:f2} seconds");
            }
        }

    }
}
