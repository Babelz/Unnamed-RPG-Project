using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.Graphics
{
    public sealed class RenderGrid
    {
        #region Fields
        private RenderCell[][] grid;

        private Vector2 cellSize;
        private Point gridSize;
        #endregion

        #region Properties
        public IEnumerable<IRenderable> Elements
        {
            get
            {
                for (int i = 0; i < gridSize.Y; i++)
                {
                    for (int j = 0; j < gridSize.X; j++)
                    {
                        foreach (var element in grid[i][j].Elements) yield return element;
                    }
                }
            }
        }
        #endregion

        public RenderGrid()
        {
        }

        public void UpdateGrid(int cellWidth, int cellHeight, int areaWidth, int areaHeight)
        {
            Debug.Assert(cellWidth != 0);
            Debug.Assert(cellHeight != 0);
            Debug.Assert(areaWidth != 0);
            Debug.Assert(areaHeight != 0);

            cellSize = new Vector2(cellWidth, cellHeight);
            gridSize = new Point((int)Math.Floor((double)areaWidth / cellWidth), (int)Math.Floor((double)areaHeight / cellHeight));

            grid = new RenderCell[gridSize.Y][];

            for (var i = 0; i < gridSize.Y; i++)
            {
                grid[i] = new RenderCell[gridSize.X];

                for (var j = 0; j < gridSize.X; j++)
                {
                    var cellPosition    = new Vector2(j * cellWidth, i * cellHeight);
                    var cell            = new RenderCell(cellPosition, cellSize);

                    grid[i][j] = cell;
                }
            }
        }

        public IEnumerable<RenderCell> VisibleCells(Vector2 viewPosition, int columnsPadding, int rowsPadding)
        {
            var startColumn     = (int)(viewPosition.X / cellSize.X) - columnsPadding;
            var startRow        = (int)(viewPosition.Y / cellSize.Y) - rowsPadding;

            startColumn         = startColumn < 0 ? 0 : startColumn;
            startRow            = startRow < 0 ? 0 : startRow;

            var endColumn = startColumn == 0    ? (int)((viewPosition.X) / cellSize.X) + columnsPadding : startColumn + columnsPadding * 2;
            var endRow    = startRow == 0       ? (int)((viewPosition.Y) / cellSize.Y) + rowsPadding    : startRow + rowsPadding * 2;

            endColumn           = endColumn > gridSize.X ? gridSize.X : endColumn;
            endRow              = endRow > gridSize.Y ? gridSize.Y : endRow;
                
            for (var i = startRow; i < endRow; i++)
            {
                for (var j = startColumn; j < endColumn; j++)
                {
                    yield return grid[i][j];
                }
            }
        }

        public IEnumerable<RenderCell> InvisibleCells(IEnumerable<RenderCell> visibleCells)
        {
            Debug.Assert(visibleCells != null);

            var visibleList = visibleCells.ToList();

            for (var i = 0; i < gridSize.Y; i++)
            {
                for (var j = 0; j < gridSize.X; i++)
                {
                    var cell = grid[i][j];

                    if (!visibleList.Contains(cell)) yield return cell; 
                }
            }
        }

        public void Add(IRenderable renderable)
        {
            Debug.Assert(renderable != null);
            Debug.Assert(!renderable.Active);

            var column  = (int)(renderable.Position.X / cellSize.X);
            var row     = (int)(renderable.Position.Y / cellSize.Y);

            if (column >= gridSize.X || column < 0)
            {
                Logger.Instance.LogFunctionWarning("grid column overlap, auto fixing to max");
                column = gridSize.X - 1;
            }

            if (row >= gridSize.Y || row < 0)
            {
                Logger.Instance.LogFunctionWarning("grid row overlap, auto fixing to max");
                row = gridSize.Y - 1;
            }

            grid[row][column].Add(renderable);

            renderable.Cell     = new Point(column, row);
            renderable.Active   = true;
        }
        public void Add(IEnumerable<IRenderable> elements)
        {
            foreach (var element in elements) Add(element);
        }
        
        public bool Remove(IRenderable renderable)
        {
            Debug.Assert(renderable != null);

            if (!renderable.Active) return false;

            var location = renderable.Cell;

            if (grid[location.Y][location.X].Remove(renderable))
            {

                renderable.Active = false;

                return true;
            }

            return false;
        }
        public int Remove(IEnumerable<IRenderable> elements)
        {
            var count = 0;

            foreach (var element in elements) if (Remove(element)) count++;

            return count;
        } 

        public void Invalidate(IRenderable renderable)
        {
            Debug.Assert(renderable != null);

            var location    = renderable.Cell;
            var cell        = grid[location.Y][location.X];

            // Still inside the same cell, return. 
            // No need to invalidate.
            if (cell.Contains(renderable)) return;

            // "invalidate", change the containing cell.
            Remove(renderable);
            Add(renderable);
        }

        public void Clear()
        {
            for (int i = 0; i < gridSize.Y; i++)
                for (int j = 0; j < gridSize.X; j++)
                    grid[i][j].Clear();
        }
    }
}
