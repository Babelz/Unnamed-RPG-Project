using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class Layer
    {
        #region Fields
        private readonly RenderGrid grid;
        #endregion

        #region Properties
        public string Name
        {
            get;
            set;
        }
        public object Key
        {
            get;
            set;
        }
        public bool Visible
        {
            get;
            set;
        }
        public Effect Effect
        {
            get;
            set;
        }
        #endregion

        private Layer(int cellWidth, int cellHeight, int areaWidth, int areaHeight)
        {
            grid = new RenderGrid();

            grid.UpdateGrid(cellWidth, cellHeight, areaWidth, areaHeight);
        }
        
        public static Layer Create(int width, int height, int cellWidth, int cellHeight, string name = null, object key = null)
        {
            return new Layer(cellWidth, cellHeight, width, height)
            {
                Name    = string.IsNullOrEmpty(name) ? string.Empty : name,
                Key     = key == null ? new object() : key
            };
        }

        public void Add(IRenderable element)
        {
            grid.Add(element);
        }
        public void Add(IEnumerable<IRenderable> elements)
        {
            grid.Add(elements);
        }

        public void Remove(IRenderable element)
        {
            grid.Remove(element);
        }
        public void Remove(IEnumerable<IRenderable> elements)
        {
            grid.Remove(elements);
        }

        public IEnumerable<IRenderable> VisibleElements(Vector2 viewPosition, Vector2 viewSize, int maxColumns, int maxRows)
        {
            foreach (var cell in grid.VisibleCells(viewPosition, viewSize, maxColumns, maxRows))
            {
                foreach (var element in cell.Elements)
                {
                    if (element.Visible) yield return element;
                }   
            }
        }
    }
}