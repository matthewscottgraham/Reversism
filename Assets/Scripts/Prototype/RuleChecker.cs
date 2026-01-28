    using UnityEngine;

    namespace Prototype
    {
        public class RuleChecker
        {
            private const int MaxRecursions = 1000;
            
            public static int[,] ApplyPostTurnRules(int[,] boardState, int currentPlayer)
            {
                return RecurseUntilNoChanges(boardState, currentPlayer);
            }

            private static int[,] RecurseUntilNoChanges(int[,] boardState, int currentPlayer)
            {
                var recursions = 0;
                while (recursions < MaxRecursions)
                {
                    // Reversi style capture of cells trapped between the current players cells
                    var stateChanged = CheckForCapturedHorizontalSpans(boardState, currentPlayer, out boardState);
                    stateChanged |= CheckForCapturedVerticalSpans(boardState, currentPlayer, out boardState);
                    
                    // tetris style line removal
                    stateChanged |= CheckForFilledRows(boardState, currentPlayer, out boardState);
                    stateChanged |= CheckForFilledColumns(boardState, currentPlayer, out boardState);
                    
                    if (!stateChanged) return boardState;
                    recursions++;
                }

                return boardState;
            }

            private static bool CheckForCapturedHorizontalSpans(int[,] boardState, int currentPlayer, out int[,] newBoardState)
            {
                newBoardState = boardState;
                for (var y = 0; y < newBoardState.GetLength(1); y++)
                {
                    for (var x = 0; x < newBoardState.GetLength(0); x++)
                    {
                        var owner = newBoardState[x, y];
                        if (owner < 0) continue;
                        var leftBoundary = FindLeftBoundaryOwner(owner, newBoardState, x, y);
                        if (leftBoundary.owner != currentPlayer) continue;
                        var rightBoundary = FindRightBoundaryOwner(owner, newBoardState, x, y);
                        if (leftBoundary.owner != rightBoundary.owner) continue;
                        if (leftBoundary.owner == owner || rightBoundary.owner == owner) continue;
                        if (leftBoundary.owner < 0 || rightBoundary.owner < 0) continue;
                        if (ChangeRowOwners(newBoardState, leftBoundary.owner, leftBoundary.index, rightBoundary.index, y))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            private static (int owner, int index) FindLeftBoundaryOwner(int middleOwner, int[,] boardState, int x, int y)
            {
                var position = x;
                while (position >= 0)
                {
                    if (boardState[position, y] != middleOwner) return (boardState[position, y], position);
                    position--;
                }
                return (middleOwner, 0);
            }
            
            private static (int owner, int index) FindRightBoundaryOwner(int middleOwner, int[,] boardState, int x, int y)
            {
                var position = x;
                while (position < boardState.GetLength(0))
                {
                    if (boardState[position, y] != middleOwner) return (boardState[position, y], position);
                    position++;
                }
                return (middleOwner, boardState.GetLength(0) - 1);
            }

            private static bool ChangeRowOwners(int[,] boardState, int newOwner, int boundsLeft, int boundsRight, int y)
            {
                var changed = false;
                for (var x = boundsLeft; x < boundsRight; x++)
                {
                    if (boardState[x, y] == newOwner) continue;
                    boardState[x, y] = newOwner;
                    changed = true;
                }

                return changed;
            }
            
            private static bool CheckForCapturedVerticalSpans(int[,] boardState, int currentPlayer, out int[,] newBoardState)
            {
                newBoardState = boardState;
                for (var y = 0; y < newBoardState.GetLength(1); y++)
                {
                    for (var x = 0; x < newBoardState.GetLength(0); x++)
                    {
                        var owner = newBoardState[x, y];
                        if (owner < 0) continue;
                        var bottomBoundary = FindBottomBoundaryOwner(owner, newBoardState, x, y);
                        if (bottomBoundary.owner != currentPlayer) continue;
                        var topBoundary = FindTopBoundaryOwner(owner, newBoardState, x, y);
                        if (bottomBoundary.owner != topBoundary.owner) continue;
                        if (bottomBoundary.owner == owner || topBoundary.owner == owner) continue;
                        if (bottomBoundary.owner < 0 || topBoundary.owner < 0) continue;
                        if (ChangeColumnOwners(newBoardState, bottomBoundary.owner, bottomBoundary.index, topBoundary.index,
                                x))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            private static (int owner, int index) FindBottomBoundaryOwner(int middleOwner, int[,] boardState, int x, int y)
            {
                var position = y;
                while (position >= 0)
                {
                    if (boardState[x, position] != middleOwner) return (boardState[x, position], position);
                    position--;
                }
                return (middleOwner, 0);
            }
            
            private static (int owner, int index) FindTopBoundaryOwner(int middleOwner, int[,] boardState, int x, int y)
            {
                var position = y;
                while (position < boardState.GetLength(1))
                {
                    if (boardState[x, position] != middleOwner) return (boardState[x, position], position);
                    position++;
                }
                return (middleOwner, boardState.GetLength(1) - 1);
            }

            private static bool ChangeColumnOwners(int[,] boardState, int newOwner, int boundsBottom, int boundsTop, int x)
            {
                var changed = false;
                for (var y = boundsBottom; y < boundsTop; y++)
                {
                    if (boardState[x, y] == newOwner) continue;
                    boardState[x, y] = newOwner;
                    changed = true;
                }
                return changed;
            }

            private static bool CheckForFilledRows(int[,] boardState, int currentPlayer, out int[,] newBoardState)
            {
                newBoardState = boardState;
                var changed = false;
                var width = newBoardState.GetLength(0);
                for (var y = 0; y < newBoardState.GetLength(1); y++)
                {
                    var fullLine = true;
                    for (var x = 0; x < width; x++)
                    {
                        if (newBoardState[x, y] == currentPlayer) continue;
                        fullLine = false;
                        break;
                    }

                    if (!fullLine) continue;
                    ChangeRowOwners(newBoardState, -1, 0, width, y);
                    changed = true;
                }
                return changed;
            }

            private static bool CheckForFilledColumns(int[,] boardState, int currentPlayer, out int[,] newBoardState)
            {
                newBoardState = boardState;
                var changed = false;
                var height = newBoardState.GetLength(1);
                for (var x = 0; x < newBoardState.GetLength(0); x++)
                {
                    var fullLine = true;
                    for (var y = 0; y < height; y++)
                    {
                        if (newBoardState[x, y] == currentPlayer) continue;
                        fullLine = false;
                        break;
                    }
                    
                    if (!fullLine) continue;
                    ChangeColumnOwners(newBoardState, -1, 0, height, x);
                    changed = true;
                }
                return changed;
            }
        }
    }