using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype
{
    public class Clicker : MonoBehaviour
    {
        public static Action<Vector2Int> OnCellClicked;
        
        private Camera _camera;
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                FindClickedCell();
            }
        }

        private void FindClickedCell()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var worldPoint = _camera.ScreenToWorldPoint(mousePosition);
            var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (!hit.collider) return;
            var cell = ClampToCell(hit.point);
            if (!InRange(cell)) return;
            OnCellClicked?.Invoke(cell);
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
