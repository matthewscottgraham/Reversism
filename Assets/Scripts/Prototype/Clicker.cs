using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    public class Clicker : MonoBehaviour
    {
        public static Action<Vector2Int> OnCellHovered;
        public static Action<Vector2Int> OnCellClicked;
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var coordinate = FindHoveredCell();
            if (Mouse.current.leftButton.wasPressedThisFrame && coordinate != null)
            {
                OnCellClicked?.Invoke(coordinate.Value);
            }
        }

        private Vector2Int? FindHoveredCell()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var worldPoint = _camera.ScreenToWorldPoint(mousePosition);
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (!hit.collider) return null;
            var cellCoordinates = ClampToCell(hit.point);
            if (!InRange(cellCoordinates)) return null;
            OnCellHovered?.Invoke(cellCoordinates);
            return cellCoordinates;
        }

        private Vector2Int ClampToCell(Vector3 hitPoint)
        {
            var x = Mathf.FloorToInt(hitPoint.x);
            var y = Mathf.FloorToInt(hitPoint.y);
            return new Vector2Int(x, y);
        }

        private bool InRange(Vector2Int cell)
        {
            if (!(cell.x >= 0) || !(cell.x < CellController.GridSize.x)) return false;
            return cell.y >= 0 && cell.y < CellController.GridSize.y;
        }
    }
}
