using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public struct TileShape
    {
        public readonly bool[,] Shape;

        public TileShape(bool[,] shape)
        {
            Shape = shape;
        }
    }
    public class TileFactory
    {
        private const int MaxTileSize = 3;
        private TileShape[] _tileShapes;
        public TileFactory()
        {
            CreateTileShapes();
        }
        
        public TileShape GetRandomTile()
        {
            return _tileShapes[Random.Range(0, _tileShapes.Length)];
        }

        private void CreateTileShapes()
        {
            var shapes = new List<TileShape>();
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, true, true },
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true },
                    { true },
                    { true }
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true },
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, true},
                    { true, true}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, true},
                    { true, false}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { false, true},
                    { true, false}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { false, true},
                    { false, false}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, true},
                    { false, true}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, false},
                    { false, true}
                }));
            
            shapes.Add(new TileShape(
                new [,]
                {
                    { true, false},
                    { true, true}
                }));
            
            _tileShapes = shapes.ToArray();
        }
    }
}
