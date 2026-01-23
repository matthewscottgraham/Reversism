using UnityEngine;

namespace Prototype
{
    public class RuleChecker : MonoBehaviour
    {
        public void Check(int[,] cells)
        {
            for (var x = 0; x < cells.GetLength(0); x++)
            {
                for (var y = 0; y < cells.GetLength(1); y++)
                {
                    // check
                }
            }
        }
    }
}