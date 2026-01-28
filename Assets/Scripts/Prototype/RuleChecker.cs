    using UnityEngine;

    namespace Prototype
    {
        public class RuleChecker
        {
            private bool _stateWasChanged = false;
            
            public int[,] ApplyPostTurnRules(int[,] boardState, int currentPlayer, Vector2Int[] changedCells)
            {
                _stateWasChanged = false;
                // Reversi style capture of cells trapped between the current players cells
                foreach (var coordinate in changedCells)
                {
                    boardState = CheckForCapturedHorizontalSpans(boardState, currentPlayer, coordinate);
                    boardState =CheckForCapturedVerticalSpans(boardState, currentPlayer, coordinate);
                }
                
                // tetris style line removal
                foreach (var coordinate in changedCells)
                {
                    boardState = CheckForFilledRows(boardState, currentPlayer, coordinate);
                    boardState = CheckForFilledColumns(boardState, currentPlayer, coordinate);
                }
                
                return _stateWasChanged ? boardState : null;
            }
            
            private int[,] CheckForCapturedHorizontalSpans(int[,] boardState, int currentPlayer, Vector2Int coordinate)
            {
                if (boardState[coordinate.x, coordinate.y] != currentPlayer) return boardState;
                
                var leftBoundary = coordinate.x - 1;
                while (leftBoundary >= 0 
                       && boardState[leftBoundary, coordinate.y] != currentPlayer)
                {
                    var owner = boardState[leftBoundary, coordinate.y];
                    if (owner < 0)
                    {
                        leftBoundary = coordinate.x;
                        break;
                    }
                    leftBoundary--;
                }
                
                var rightBoundary = coordinate.x + 1;
                while (rightBoundary < boardState.GetLength(0) 
                       && boardState[rightBoundary, coordinate.y] != currentPlayer)
                {
                    var owner = boardState[rightBoundary, coordinate.y];
                    if (owner < 0)
                    {
                        rightBoundary = coordinate.x;
                        break;
                    }
                    rightBoundary++;
                }
                
                // If there is no distance between the boundaries, there is nothing to capture;
                if (leftBoundary - rightBoundary == 0) return boardState;
                
                ChangeRowOwners(boardState, currentPlayer, leftBoundary + 1, rightBoundary, coordinate.y);
                return boardState;
            }

            private void ChangeRowOwners(int[,] boardState, int newOwner, int boundsLeft, int boundsRight, int y)
            {
                var changed = false;
                for (var x = boundsLeft; x < boundsRight; x++)
                {
                    if (boardState[x, y] == newOwner) continue;
                    boardState[x, y] = newOwner;
                    changed = true;
                }
                _stateWasChanged = changed || _stateWasChanged;
            }

            private int[,] CheckForCapturedVerticalSpans(int[,] boardState, int currentPlayer, Vector2Int coordinate)
            {
                if (boardState[coordinate.x, coordinate.y] != currentPlayer) return boardState;
                
                var bottomBoundary = coordinate.y - 1;
                while (bottomBoundary >= 0 
                       && boardState[coordinate.x, bottomBoundary] != currentPlayer)
                {
                    var owner = boardState[coordinate.x, bottomBoundary];
                    if (owner < 0)
                    {
                        bottomBoundary = coordinate.y;
                        break;
                    }
                    bottomBoundary--;
                }
                
                var topBoundary = coordinate.y + 1;
                while (topBoundary < boardState.GetLength(1) 
                       && boardState[coordinate.x, topBoundary] != currentPlayer)
                {
                    var owner = boardState[coordinate.x, topBoundary];
                    if (owner < 0)
                    {
                        topBoundary = coordinate.y;
                        break;
                    }
                    topBoundary++;
                }
                
                // If there is no distance between the boundaries, there is nothing to capture;
                if (bottomBoundary - topBoundary == 0) return boardState;
                
                ChangeColumnOwners(boardState, currentPlayer, bottomBoundary + 1, topBoundary, coordinate.x);
                return boardState;
            }

            private void ChangeColumnOwners(int[,] boardState, int newOwner, int boundsBottom, int boundsTop, int x)
            {
                var changed = false;
                for (var y = boundsBottom; y < boundsTop; y++)
                {
                    if (boardState[x, y] == newOwner) continue;
                    boardState[x, y] = newOwner;
                    changed = true;
                }
                _stateWasChanged = changed || _stateWasChanged;
            }

            private int[,] CheckForFilledRows(int[,] boardState, int currentPlayer, Vector2Int coordinate)
            {
                var width = boardState.GetLength(0);
                var fullLine = true;
                for (var x = 0; x < width; x++)
                {
                    if (boardState[x, coordinate.y] == currentPlayer) continue;
                    fullLine = false;
                    break;
                }

                if (!fullLine) return boardState;
                ChangeRowOwners(boardState, -1, 0, width, coordinate.y);
                _stateWasChanged = true;
                return boardState;
            }

            private int[,] CheckForFilledColumns(int[,] boardState, int currentPlayer, Vector2Int coordinate)
            {
                var height = boardState.GetLength(1);
                var fullLine = true;
                for (var y = 0; y < height; y++)
                {
                    if (boardState[coordinate.x, y] == currentPlayer) continue;
                    fullLine = false;
                    break;
                }
                
                if (!fullLine) return boardState;
                ChangeColumnOwners(boardState, -1, 0, height, coordinate.x);
                _stateWasChanged = true;
                return boardState;
            }
        }
    }