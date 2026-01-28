using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;
        private Dictionary<Vector2Int, Cell> _cellDictionary;
        private HashSet<Cell> _hoveredCells = new();
        private RuleChecker _ruleChecker;
        
        public static Vector2Int GridSize => new Vector2Int(12, 12);
        
        private void UpdateBoardState(int[,] boardState)
        {
            if (boardState == null) return;
            for (var x = 0; x < boardState.GetLength(0); x++)
            {
                for (var y = 0; y < boardState.GetLength(1); y++)
                {
                    if (_cellDictionary.TryGetValue(new Vector2Int(x, y), out var cell))
                    {
                        cell.SetOwner(boardState[x, y]);
                    }
                }
            }
        }
        
        private void Awake()
        {
            _ruleChecker = new RuleChecker();
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
            UpdateBoardState(_ruleChecker.ApplyPostTurnRules(GetBoardState(), currentPlayer, GetHoveredCoordinates()));
            PlayerController.IncrementPlayer();
        }

        private Vector2Int[] GetHoveredCoordinates()
        {
            var coordinates = new Vector2Int[_hoveredCells.Count];
            var index = 0;
            foreach (var hoveredCell in _hoveredCells)
            {
                coordinates[index] = hoveredCell.Coordinate;
                index++;
            }

            return coordinates;
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
            bool inBounds = true;
            for (var x = 0; x < tiles.GetLength(0); x++)
            {
                for (var y = 0; y < tiles.GetLength(1); y++)
                {
                    if (!tiles[x, y]) continue;
                    var adjustedCoordinate = new Vector2Int(x + coordinate.x, y + coordinate.y);
                    if (!_cellDictionary.TryGetValue(adjustedCoordinate, out var value))
                    {
                        inBounds = false;
                        continue;
                    }
                    if (!value.IsClaimable) return;
                    hoveredCells.Add(value);
                }
            }
            if (!inBounds) return;
            _hoveredCells = hoveredCells;
        }

        private int[,] GetBoardState()
        {
            var board = new int[GridSize.x, GridSize.y];
            for (var x = 0; x < board.GetLength(0); x++)
            {
                for (var y = 0; y < board.GetLength(1); y++)
                {
                    if (_cellDictionary.TryGetValue(new Vector2Int(x, y), out var cell))
                    {
                        board[x, y] = cell.Owner;
                    }
                    else
                    {
                        board[x, y] = -1;
                    }
                }
            }
            return board;
        }
    }
}