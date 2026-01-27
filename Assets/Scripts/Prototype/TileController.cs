using UnityEngine;

namespace Prototype
{
    public class TileController : MonoBehaviour
    {
        public static TileShape CurrentTile;
        
        private readonly TileFactory _tileFactory = new();

        private void Start()
        {
            DrawNewTile();
        }

        private void OnEnable()
        {
            PlayerController.OnPlayerChanged += DrawNewTile;
        }

        private void OnDisable()
        {
            PlayerController.OnPlayerChanged -= DrawNewTile;
        }

        private void DrawNewTile()
        {
            CurrentTile = _tileFactory.GetRandomTile();
        }
    }
}