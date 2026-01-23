using UnityEngine;

namespace Prototype
{
    public static class PlayerController
    {
        private const int NumberOfPlayers = 2;

        private static readonly Color[] PlayerColours = new[]
        {
            new Color(0, 0, 255, 255),
            new Color(255, 0, 0, 255),
        };

        public static int CurrentPlayer { get; private set; } = 0;
        public static Color CurrentPlayerColour => PlayerColours[CurrentPlayer];

        public static void IncrementPlayer()
        {
            ++CurrentPlayer;
            CurrentPlayer %= NumberOfPlayers;
        }
    }
}