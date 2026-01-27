using UnityEngine;

namespace Prototype
{
    public static class PlayerController
    {
        public static System.Action<int, float> OnPlayerScoreChange;
        private const int NumberOfPlayers = 2;

        private static readonly Color[] PlayerColours = new[]
        {
            new Color(0, 0, 255, 255),
            new Color(255, 0, 0, 255),
        };


        public static int[] Score { get; private set; } = new int[NumberOfPlayers];
        public static int CurrentPlayer { get; private set; } = 0;
        public static Color CurrentPlayerColour => PlayerColours[CurrentPlayer];

        public static void IncrementPlayer()
        {
            ++CurrentPlayer;
            CurrentPlayer %= NumberOfPlayers;
        }

        public static Color GetPlayerColour(int player)
        {
            if (player < 0 || player >= NumberOfPlayers) return Color.clear;
            return PlayerColours[player];
        }

        public static void IncrementScore(int player, int increment)
        {
            if (player < 0 || player >= NumberOfPlayers) return;
            Score[player] += increment;
            if (Score[player] < 0) Score[player] = 0;
            OnPlayerScoreChange?.Invoke(player, (float)Score[player] / CellController.MaxScore);
        }
    }
}