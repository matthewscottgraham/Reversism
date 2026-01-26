using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prototype
{
    public class CellController : MonoBehaviour
    {
        public static Vector2Int GridSize  = new Vector2Int(12, 12);

        [FormerlySerializedAs("_sprite")] [SerializeField] private Sprite sprite;
        private Dictionary<Vector2Int, Cell> _cellDictionary;
        private HashSet<Cell> _hoveredCells = new();

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
            
            var cell = go.AddComponent<Cell>();
            return cell;
        }

        private void HandleCellClicked(Vector2Int cell)
        {
            var currentPlayer = PlayerController.CurrentPlayer;
            if (!ClaimCell(cell, currentPlayer)) return;
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
            
            _hoveredCells.Add(_cellDictionary[coordinate]);
            foreach (var hoveredCell in _hoveredCells)
            {
                hoveredCell.Hover();
            }
        }

        private bool ClaimCell(Vector2Int cell, int player)
        {
            return _cellDictionary[cell].Claim(player);
        }
    }
}