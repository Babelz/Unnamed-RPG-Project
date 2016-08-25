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
    public sealed class Renderer : SystemManager<Renderer>
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
        public int UserColumnsPadding
        {
            get;
            set;
        }
        public int UserRowsPadding
        {
            get;
            set;
        }
        public bool DynamicPadding
        {
            get;
            set;
        }
        public int LayerHeight
        {
            get
            {
                return layerHeight;
            }
        }
        public int LayerWidth
        {
            get
            {
                return layerWidth;
            }
        }
        #endregion

        private Renderer()
            : base()
        {
            spriteBatch     = new SpriteBatch(Engine.Instance.GraphicsDevice);
            views           = new List<View>();
            reservedIndices = new List<int>();
            freeIndices     = new Stack<int>();
            layers          = new Layer[InitialLayersCount];
            ClearColor      = Color.CornflowerBlue;
        }
        
        private void Present(GameTime gameTime)
        {
            visibleElements = 0;

            var device      = Engine.Instance.GraphicsDevice;
            var viewport    = device.Viewport;

            device.Clear(ClearColor);

            foreach (var view in views)
            {
                device.Viewport = view.Viewport;
                
                var viewPosition    = view.Position;
                var viewSize        = view.VisibleArea;

                viewSize.X = viewSize.X <= viewport.Width  ? viewport.Width  : viewSize.X;
                viewSize.Y = viewSize.Y <= viewport.Height ? viewport.Height : viewSize.Y;

                var columnsPadding  = DynamicPadding ? (int)Math.Floor(viewSize.X / cellWidth)      : UserColumnsPadding;
                var rowsPadding     = DynamicPadding ? (int)Math.Floor(viewSize.Y / cellHeight) + 1 : UserRowsPadding;

                for (var i = 0; i < reservedIndices.Count; i++)
                {
                    var layer = layers[reservedIndices[i]];

                    if (!layer.Visible) continue;

                    spriteBatch.Begin(SpriteSortMode.FrontToBack,
                                      BlendState.AlphaBlend,
                                      null,
                                      null,
                                      null,
                                      layer.Effect,
                                      view.Transform);

                    foreach (var element in layer.VisibleElements(viewPosition, columnsPadding, rowsPadding))
                    {
                        element.Present(spriteBatch, gameTime);

                        visibleElements++;
                    }

                    spriteBatch.End();
                }
            }

            device.Viewport = viewport;
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            Present(gameTime);
        }

        public void SetPresentationParameters(int layerWidth, int layerHeight, int cellWidth, int cellHeight, int columnsPadding = 3, int rowsPadding = 3)
        {
            this.layerWidth     = layerWidth;
            this.layerHeight    = layerHeight;

            this.cellWidth      = cellWidth;
            this.cellHeight     = cellHeight;

            UserColumnsPadding  = columnsPadding;
            UserRowsPadding     = rowsPadding;
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
        public void DestroyLayer(int layer)
        {
            if (layers[layer] == null || layer >= layers.Length) throw new vRPGEngineException("layers access violation - invalid id");

            freeIndices.Push(layer);
            layers[layer] = null;
        }

        public void ShowLayer(int layer)
        {
            if (layers[layer] == null || layer >= layers.Length) throw new vRPGEngineException("layers access violation - invalid id");

            layers[layer].Visible = true;
        }
        public void HideLayer(int layer)
        {
            if (layers[layer] == null || layer >= layers.Length) throw new vRPGEngineException("layers access violation - invalid id");

            layers[layer].Visible = false;
        }

        public void ClearLayer(int layer)
        {
            if (layers[layer] == null || layer >= layers.Length) throw new vRPGEngineException("layers access violation - invalid id");

            layers[layer].Clear();
        }

        public void ClearLayers()
        {
            freeIndices.Clear();

            layers = new Layer[InitialLayersCount];

            ptr = 0;
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
        public IEnumerable<View> Views()
        {
            for (var i = 0; i < views.Count; i++) yield return views[i];
        }

        public void Invalidate(IRenderable element)
        {
            if (!element.Active) return;
            
            layers[element.Layer].Invalidate(element);
        }

        public bool HasLayers()
        {
            return layers.FirstOrDefault(l => l != null) != null;
        }
    }
}
