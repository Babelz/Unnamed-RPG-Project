using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;

namespace vRPGEngine.Graphics
{
    public sealed class Renderer : SystemManager<Renderer>
    {
        #region Constants
        private const int InitialLayersCount    = 4;
        private const int MaxLayers             = 32;
        #endregion

        #region Fields
        private readonly PenumbraComponent penumbra;
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
            penumbra        = new PenumbraComponent(Engine.Instance.Game);
            spriteBatch     = new SpriteBatch(Engine.Instance.GraphicsDevice);
            views           = new List<View>();
            reservedIndices = new List<int>();
            freeIndices     = new Stack<int>();
            layers          = new Layer[InitialLayersCount];
            ClearColor      = Color.CornflowerBlue;

            penumbra.Initialize();
            penumbra.AmbientColor                   = Color.Black;
            penumbra.SpriteBatchTransformEnabled    = true;
            penumbra.AmbientColor                   = Color.White;
        }
        
        private void Present(GameTime gameTime)
        {
            penumbra.BeginDraw();
            
            visibleElements = 0;

            var device      = Engine.Instance.GraphicsDevice;
            var viewport    = device.Viewport;

            device.Clear(ClearColor);

            foreach (var view in views)
            {
                penumbra.Transform  = view.Transform;
                device.Viewport     = view.Viewport;
                
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
                                      SamplerState.PointClamp,
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
                
                penumbra.Draw(gameTime);
            }

            device.Viewport = viewport;
        }
        
        protected override void OnUpdate(GameTime gameTime)
        {
            penumbra.Update(gameTime);

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
            
            if (layers[element.Layer].Remove(element)) totalElements--;
        }
        public void Remove(IEnumerable<IRenderable> elements)
        {
            Debug.Assert(elements != null);
            
            foreach (var element in elements) if(layers[element.Layer].Remove(element)) totalElements--;
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

        public Hull CreateHull(Vector2 position, Vector2[] points)
        {
            Debug.Assert(points != null);

            var hull = new Hull(points);

            hull.Position = position;
            hull.Enabled = true;

            if (!hull.Valid)
            {
                Logger.Instance.LogError("invalid points for hull! could not create hull!");

                return null;
            }

            return hull;
        }
        public void RemoveHull(Hull hull)
        {
            Debug.Assert(hull != null);
            Debug.Assert(hull.Valid);

            penumbra.Hulls.Remove(hull);
        }
        public void AddHull(Hull hull)
        {
            Debug.Assert(hull != null);
            Debug.Assert(hull.Valid);

            penumbra.Hulls.Add(hull);
        }

        public Spotlight CreateSpotLight(ShadowType shadowType)
        {
            var light = new Spotlight();

            light.ShadowType = shadowType;

            return light;
        }
        public PointLight CreatePointLight(ShadowType shadowType)
        {
            var light = new PointLight();

            light.ShadowType = shadowType;

            return light;
        }
        public void RemoveLight(Light light)
        {
            Debug.Assert(light != null);

            penumbra.Lights.Remove(light);
        }
        public void AddLight(Light light)
        {
            Debug.Assert(light != null);

            penumbra.Lights.Add(light);
        }

        public IEnumerable<T> Query<T>(Func<IRenderable, T> selector) where T : class
        {
            foreach (var layer in layers.Where(l => l != null))
            {
                foreach (var element in layer.Elements)
                {
                    var results = selector(element);

                    if (results != null) yield return results;
                }
            }
        }
    }
}
