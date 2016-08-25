using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Layer(int cellWidth, int cellHeight, int areaWidth, int areaHeight)
        {
            grid = new RenderGrid();

            grid.UpdateGrid(cellWidth, cellHeight, areaWidth, areaHeight);
        }

        public void Add(IRenderable element)
        {
            Debug.Assert(element != null);
            
            grid.Add(element);
        }
        public void Add(IEnumerable<IRenderable> elements)
        {
            Debug.Assert(elements != null);

            grid.Add(elements);
        }

        public void Remove(IRenderable element)
        {
            Debug.Assert(element != null);

            grid.Remove(element);
        }
        public void Remove(IEnumerable<IRenderable> elements)
        {
            Debug.Assert(elements != null);

            grid.Remove(elements);
        }

        public IEnumerable<IRenderable> VisibleElements(Vector2 viewPosition, int columnsPadding, int rowsPadding)
        {
            foreach (var cell in grid.VisibleCells(viewPosition, columnsPadding, rowsPadding))
            {
                foreach (var element in cell.Elements)
                {
                    if (element.Visible) yield return element;
                }   
            }
        }

        public void Invalidate(IRenderable element)
        {
            Debug.Assert(element != null);

            grid.Invalidate(element);
        }

        public void Clear()
        {
            grid.Clear();
        }
    }
}