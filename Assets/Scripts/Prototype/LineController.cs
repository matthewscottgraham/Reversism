using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Prototype
{
    public class LineController : MonoBehaviour
    {
        private static readonly Vector2 LineWidthRange = new Vector2(0.005f, 0.02f);
        private const float PointDisplacementAmount = 2f;
        private void Start()
        {
            CreateLines(CellController.GridSize.x, CellController.GridSize.y);
        }

        private void CreateLines(int width, int height)
        {
            var childObject = new GameObject("Lines");
            childObject.transform.SetParent(transform, false);

            for (var x = 0; x < width; x++)
            {
                var lineRenderer = CreateLineRenderer(childObject);
                lineRenderer.SetPosition(0, new Vector3(x, DisplacePoint(0), 0));
                lineRenderer.SetPosition(1, new Vector3(x, DisplacePoint(height), 0));
            }
            
            for (var y = 0; y < height; y++)
            {
                var lineRenderer = CreateLineRenderer(childObject);
                lineRenderer.SetPosition(0, new Vector3(DisplacePoint(0), y, 0));
                lineRenderer.SetPosition(1, new Vector3(DisplacePoint(width), y, 0));
            }
        }

        private static LineRenderer CreateLineRenderer(GameObject childObject)
        {
            var lineObject = new GameObject();
            lineObject.transform.SetParent(childObject.transform, false);
            var lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = Random.Range(LineWidthRange.x, LineWidthRange.y);
            lineRenderer.endWidth = Random.Range(LineWidthRange.x, LineWidthRange.y);
            lineRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = new Color(1,1,1, Random.value);
            lineRenderer.endColor = new Color(1,1,1, Random.value);
            return lineRenderer;
        }

        private float DisplacePoint(float basePosition)
        {
            return Random.Range(-PointDisplacementAmount, PointDisplacementAmount) + basePosition;
        }
    }
}
