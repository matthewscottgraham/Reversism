using UnityEngine;

namespace Prototype
{
    public static class PlayerController
    {
        public static System.Action<int, int> OnPlayerScoreChange;
        public static System.Action OnPlayerChanged;
        
        private const int NumberOfPlayers = 2;

        private static readonly Color[] PlayerColours = new[]
        {
            new Color(0.26f, 0.26f, 0.86f, 1),
            new Color(0.80f, 0.21f, 0.21f, 1),
        };

        private static readonly Color[] HoverColours = new[]
        {
            new Color(0.46f, 0.46f, 0.96f, 1),
            new Color(0.90f, 0.41f, 0.41f, 1),
        };

        public static int ScoreMultiplier { get; private set; } = 1;
        public static int[] Score { get; private set; } = new int[NumberOfPlayers];
        public static int CurrentPlayer { get; private set; } = 0;
        public static Color CurrentPlayerColour => PlayerColours[CurrentPlayer];
        public static Color CurrentHoverColour => HoverColours[CurrentPlayer];

        public static void IncrementMultiplier()
        {
            ScoreMultiplier++;
        }
        public static void IncrementPlayer()
        {
            ++CurrentPlayer;
            CurrentPlayer %= NumberOfPlayers;
            ScoreMultiplier = 1;
            OnPlayerChanged?.Invoke();
        }

        public static Color GetPlayerColour(int player)
        {
            if (player < 0 || player >= NumberOfPlayers) return Color.clear;
            return PlayerColours[player];
        }

        public static void IncrementScore(int player, int increment)
        {
            if (player < 0 || player >= NumberOfPlayers) return;
            Score[player] += increment * ScoreMultiplier;
            if (Score[player] < 0) Score[player] = 0;
            if (Score[player] >= 1000) Debug.Log($"Player {player} has {Score[player]} points.");
            OnPlayerScoreChange?.Invoke(player, Score[player]);
        }
    }
}