using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.Graphics
{
    public sealed class RenderCell
    {
        #region Fields
        private readonly Vector2 position;
        private readonly Vector2 size;

        private readonly List<int> reservedIndices;
        private readonly Stack<int> freeIndices;

        private IRenderable[] elements;
        #endregion

        #region Properties
        public int ElementsCount
        {
            get
            {
                return reservedIndices.Count;
            }
        }
        public IEnumerable<IRenderable> Elements
        {
            get
            {
                for (var i = 0; i < reservedIndices.Count; i++) yield return elements[reservedIndices[i]];
            }
        }
        #endregion

        public RenderCell(Vector2 position, Vector2 size)
        {
            const int InitialCapacity = 128;

            this.position   = position;
            this.size       = size;

            reservedIndices = new List<int>();
            freeIndices     = new Stack<int>();
            elements        = new IRenderable[InitialCapacity];

            for (var i = 0; i < InitialCapacity; i++) freeIndices.Push(i);
        }

        private void ResizeStorage()
        {
            var newElements = new IRenderable[elements.Length * 2];
            var start       = elements.Length - 1;
            var end         = newElements.Length;

            // Gen new indices.
            for (var i = start; i < end; i++) freeIndices.Push(i);

            // Copy and store.
            Array.Copy(elements, newElements, elements.Length);

            elements = newElements;
        }

        public bool InView(Vector2 viewPosition, Vector2 viewSize)
        {
            return VectorExtensions.Intersects(this.position,
                                               this.size,
                                               viewPosition,
                                               viewSize);
        }
        
        public void Add(IRenderable renderable)
        {
            Debug.Assert(renderable != null);

            if (freeIndices.Count == 0) ResizeStorage();

            // Store.
            var index           = freeIndices.Pop();
            renderable.Index    = index;
            elements[index]     = renderable;

            // Reserve.
            reservedIndices.Add(index);
        }
        public void Remove(IRenderable renderable)
        {
            Debug.Assert(renderable != null);

            var index = renderable.Index;

            if (index >= elements.Length || elements[index] != null)
            {
                Logger.Instance.LogFunctionWarning("cell access violation - element not found");

                return;
            }

            // "remove" element.
            elements[index] = null;

            // Store new free index.
            freeIndices.Push(index);
            // Remove reservation.
            reservedIndices.Remove(index);
        }
        
        public bool Inside(IRenderable renderable)
        {
            if (renderable.Index >= elements.Length || elements[renderable.Index] != null) return false;

            return VectorExtensions.Intersects(position,
                                               size,
                                               renderable.Position,
                                               renderable.Size);
        }
    }
}
