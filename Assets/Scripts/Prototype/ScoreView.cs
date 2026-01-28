using UnityEngine;
using UnityEngine.UI;

namespace Prototype
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private Slider[] playerProgress = new Slider[2];

        private void OnEnable()
        {
            PlayerController.OnPlayerScoreChange += HandlePlayerScoreChange;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerScoreChange -= HandlePlayerScoreChange;
        }

        private void HandlePlayerScoreChange(int playerIndex, int newScore)
        {
            playerProgress[playerIndex].value = newScore;
        }
    }
}
