using System;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class CellController : MonoBehaviour
    {
        public static Vector2Int GridSize  = new Vector2Int(12, 12);

        [SerializeField] private Sprite _sprite;
        
        private int[,] _cells;
        private Dictionary<Vector2Int, SpriteRenderer> _spriteRenderers;

        private void Awake()
        {
            InitializeCells();
        }

        private void OnEnable()
        {
            Clicker.OnCellClicked += HandleCellClicked;
        }

        private void OnDisable()
        {
            Clicker.OnCellClicked -= HandleCellClicked;
        }

        private void InitializeCells()
        {
            _spriteRenderers = new Dictionary<Vector2Int, SpriteRenderer>();
            _cells = new int[GridSize.x, GridSize.y];
            
            for (var x = 0; x < _cells.GetLength(0); x++)
            {
                for (var y = 0; y < _cells.GetLength(1); y++)
                {
                    _cells[x, y] = -1;
                    _spriteRenderers.Add(new Vector2Int(x,y), CreateCellObject(x, y));
                }
            }
        }

        private SpriteRenderer CreateCellObject(int x, int y)
        {
            var go = new GameObject();
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f);
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = _sprite;
            spriteRenderer.color = Color.clear;
            return spriteRenderer;
        }

        private void HandleCellClicked(Vector2Int cell)
        {
            var currentPlayer = PlayerController.CurrentPlayer;
            if (!ClaimCell(cell, currentPlayer)) return;
            PlayerController.IncrementPlayer();
        }

        private bool ClaimCell(Vector2Int cell, int player)
        {
            if (_cells[cell.x, cell.y] >= 0) return false; // already claimed
            _cells[cell.x, cell.y] = player;
            _spriteRenderers[cell].color = PlayerController.CurrentPlayerColour;
            return true;
        }
    }
}