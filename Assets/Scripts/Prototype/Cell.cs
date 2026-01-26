using UnityEngine;

namespace Prototype
{
    public class Cell : MonoBehaviour
    {
        private int _owner = -1;
        private Renderer _renderer;
        private static readonly Color HoverColor = Color.cyan;
        
        public void Hover()
        {
            if (_owner >= 0 ) return;
            SetColor(HoverColor);
        }

        public void DeHover()
        {
            if (_owner >= 0 ) return;
            SetColor(PlayerController.GetPlayerColour(_owner));
        }

        public bool Claim(int owner)
        {
            if (_owner >= 0 ) return false;
            _owner = owner;
            SetColor(PlayerController.GetPlayerColour(_owner));
            return true;
        }

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            SetColor(Color.clear);
        }

        private void SetColor(Color color)
        {
            _renderer.material.color = color;
        }
    }
}