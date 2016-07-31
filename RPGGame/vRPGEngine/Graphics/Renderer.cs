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
    public sealed class Renderer : Singleton<Renderer>
    {
        #region Constants
        private const int InitialLayersCount    = 4;
        private const int MaxLayers             = 32;
        #endregion

        #region Fields
        private readonly SpriteBatch spriteBatch;
        private readonly List<View> views;
        private readonly List<int> reservedIndices;
        private readonly Stack<int> freeIndices;

        private Layer[] layers;
        private int ptr;

        private int layerWidth;
        private int layerHeight;

        private int cellWidth;
        private int cellHeight;

        private int maxColumns;
        private int maxRows;

        private int totalElements;
        private int visibleElements;
        #endregion

        #region Properties
        public Color ClearColor
        {
            get;
            set;
        }
        public int TotalElements
        {
            get
            {
                return totalElements;
            }
        }
        public int VisibleElements
        {
            get
            {
                return visibleElements;
            }
        }
        #endregion

        private Renderer()
            : base()
        {
            spriteBatch     = new SpriteBatch(vRPGEngine.Instance.GraphicsDevice);
            views           = new List<View>();
            reservedIndices = new List<int>();
            freeIndices     = new Stack<int>();
            layers          = new Layer[InitialLayersCount];
            ClearColor      = Color.CornflowerBlue;
        }
        
        private void InternalPresent(View view)
        {
            var viewSize        = new Vector2(view.Viewport.Width * view.Zoom, view.Viewport.Height * view.Zoom);
            var viewTransform   = view.Transform();
            var viewPosition    = view.Position;

            for (var i = 0; i < reservedIndices.Count; i++)
            {
                var layer = layers[reservedIndices[i]];

                spriteBatch.Begin(SpriteSortMode.Deferred,
                                  BlendState.AlphaBlend,
                                  SamplerState.PointClamp,
                                  null,
                                  null,
                                  layer.Effect,
                                  view.Transform());

                foreach (var element in layer.VisibleElements(viewPosition, viewSize, maxColumns, maxRows))
                {
                    element.Present(spriteBatch);

                    visibleElements++;
                }

                spriteBatch.End();
            }
        }

        public void SetPresentationParameters(int layerWidth, int layerHeight, int cellWidth, int cellHeight, int maxColumns = 3, int maxRows = 3)
        {
            this.layerWidth     = layerWidth;
            this.layerHeight    = layerHeight;

            this.cellWidth      = cellWidth;
            this.cellHeight     = cellHeight;

            this.maxColumns     = maxColumns;
            this.maxRows        = maxRows;
        }

        public void Add(IRenderable element, int layer)
        {
            Debug.Assert(element != null);

            totalElements++;

            layers[layer].Add(element);

            element.Layer = layer;
        }
        public void Add(IEnumerable<IRenderable> elements, int layer)
        {
            Debug.Assert(elements != null);

            totalElements += elements.Count();

            layers[layer].Add(elements);

            foreach (var element in elements) element.Layer = layer;
        }

        public void Remove(IRenderable element)
        {
            Debug.Assert(element != null);

            totalElements--;

            layers[element.Layer].Remove(element);
        }
        public void Remove(IEnumerable<IRenderable> elements)
        {
            Debug.Assert(elements != null);

            totalElements -= elements.Count();

            foreach (var element in elements) layers[element.Layer].Remove(element);
        }

        public int CreateLayer()
        {
            var index   = 0;
            Layer layer = null;

            if (freeIndices.Count != 0)
            {   
                reservedIndices.Add(index);
                layers[index] = layer;

                return index;
            }

            if (ptr + 1 >= layers.Length)
            {
                if (layers.Length * 2 > MaxLayers) Logger.Instance.LogFunctionWarning("layers count! " + layers.Length * 2);

                Array.Resize(ref layers, layers.Length * 2);
            }

            index = ptr;
            layer = new Layer(cellWidth, cellHeight, layerWidth, layerHeight);

            reservedIndices.Add(index);
            layers[ptr++] = layer;

            return index;
        }
        public void DestroyLayer(int id)
        {
            if (layers[id] == null || id >= layers.Length) throw new vRPGEngineException("layers access violation - invalid id");

            freeIndices.Push(id);
            layers[id] = null;
        }

        public void ClearLayers()
        {
            freeIndices.Clear();

            layers = new Layer[InitialLayersCount];
        }

        public void RegisterView(View view)
        {
            Debug.Assert(view != null);

            if (views.Contains(view)) return;

            views.Add(view);
        }
        public void UnregisterView(View view)
        {
            Debug.Assert(view != null);

            views.Remove(view);
        }

        public void Present()
        {
            visibleElements = 0;

            var device = vRPGEngine.Instance.GraphicsDevice;

            device.Clear(ClearColor);
            
            foreach (var view in views)
            {
                device.Viewport = view.Viewport;

                InternalPresent(view);
            }
        }
    }
}
