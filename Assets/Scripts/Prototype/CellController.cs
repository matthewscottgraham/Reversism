using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prototype
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;
        private Dictionary<Vector2Int, Cell> _cellDictionary;
        private HashSet<Cell> _hoveredCells = new();
        
        public static Vector2Int GridSize => new Vector2Int(12, 12);
        public static int MaxScore => GridSize.x * GridSize.y;

        private void Awake()
        {
            InitializeCells();
        }

        private void OnEnable()
        {
            Clicker.OnCellClicked += HandleCellClicked;
            Clicker.OnCellHovered += HandleCellHovered;
        }

        private void OnDisable()
        {
            Clicker.OnCellClicked -= HandleCellClicked;
            Clicker.OnCellHovered -= HandleCellHovered;
        }

        private void InitializeCells()
        {
            _cellDictionary = new Dictionary<Vector2Int, Cell>();
            
            for (var x = 0; x < GridSize.x; x++)
            {
                for (var y = 0; y < GridSize.y; y++)
                {
                    _cellDictionary.Add(new Vector2Int(x,y), CreateCellObject(x, y));
                }
            }
        }

        private Cell CreateCellObject(int x, int y)
        {
            var go = new GameObject();
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f);
            
            var spriteRenderer = go.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingOrder = 1;
            
            var cell = go.AddComponent<Cell>();
            return cell;
        }

        private void HandleCellClicked(Vector2Int coordinate)
        {
            var currentPlayer = PlayerController.CurrentPlayer;
            if (!ClaimHoveredCells(currentPlayer)) return;
            PlayerController.IncrementPlayer();
        }

        private void HandleCellHovered(Vector2Int coordinate)
        {
            if (_hoveredCells.Count > 0)
            {
                foreach (var hoveredCell in _hoveredCells)
                {
                    hoveredCell.DeHover();
                }
                _hoveredCells.Clear();
            }

            GetHoveredCells(coordinate);
            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.Hover();
            }
        }

        private bool ClaimHoveredCells(int player)
        {
            if (_hoveredCells.Count == 0) return false;
            foreach (var hoveredCell in _hoveredCells)
            {
                if (!hoveredCell.IsClaimable) return false;
            }

            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.Claim(player);
            }
            return true;
        }

        private void GetHoveredCells(Vector2Int coordinate)
        {
            var tiles = TileController.CurrentTile.Shape;
            HashSet<Cell> hoveredCells = new();
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    if (!tiles[x, y]) continue;
                    var adjustedCoordinate = new Vector2Int(x + coordinate.x, y + coordinate.y);
                    if (!_cellDictionary.TryGetValue(adjustedCoordinate, out var value)) continue;
                    if (!value.IsClaimable) return;
                    hoveredCells.Add(value);
                }
            }
            _hoveredCells = hoveredCells;
        }
    }
}