using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class RenderGrid
    {
        #region Fields
        private RenderCell[][] grid;

        private Vector2 cellSize;
        private Point gridSize;
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
            gridSize = new Point(areaWidth / cellWidth, areaHeight / cellHeight);

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

        public IEnumerable<RenderCell> VisibleCells(Vector2 viewPosition, Vector2 viewSize, int maxColumns, int maxRows)
        {
            Debug.Assert(maxRows != 0);
            Debug.Assert(maxColumns != 0);

            var startColumn     = (int)(viewPosition.X / cellSize.X);
            var startRow        = (int)(viewPosition.Y / cellSize.Y);

            var endColumn       = startColumn + maxColumns;
            var endRow          = startRow + maxRows;

            endColumn           = endColumn > gridSize.X ? gridSize.X : endColumn;
            endRow              = endRow > gridSize.Y ? gridSize.Y : endRow;
                
            for (var i = startRow; i < endRow; i++)
            {
                for (var j = startColumn; j < endColumn; j++)
                {
                    var cell = grid[i][j];

                    if (cell.InView(viewPosition, viewSize)) yield return cell;
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

            grid[row][column].Add(renderable);

            renderable.Cell = new Point(column, row);
            renderable.Active = true;
        }
        public void Add(IEnumerable<IRenderable> elements)
        {
            foreach (var element in elements) Add(element);
        }
        
        public void Remove(IRenderable renderable)
        {
            Debug.Assert(renderable != null);
            Debug.Assert(renderable.Active);

            var location = renderable.Cell;

            grid[location.Y][location.X].Remove(renderable);

            renderable.Active = false;
        }
        public void Remove(IEnumerable<IRenderable> elements)
        {
            foreach (var element in elements) Remove(element);
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
    }
}
