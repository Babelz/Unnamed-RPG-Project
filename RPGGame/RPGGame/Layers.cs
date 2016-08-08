using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Graphics;

namespace RPGGame
{
    public static class Layers
    {
        #region Fields
        public const int MapLayer           = 0;
        public const int EntityLayerBottom  = 1;
        public const int EntityLayerMiddle  = 2;
        public const int EntityLayerTop     = 3;
        public const int EffectsLayer       = 4;
        public const int HUDLayer           = 5;
        #endregion

        public static void Create()
        {
            if (Renderer.Instance.HasLayers()) Renderer.Instance.ClearLayers();
            
            Renderer.Instance.CreateLayer();    // Map layer.
            Renderer.Instance.CreateLayer();    // Entity layer bottom.
            Renderer.Instance.CreateLayer();    // Entity layer middle.
            Renderer.Instance.CreateLayer();    // Entity layer top.
            Renderer.Instance.CreateLayer();    // Effects layer.
            Renderer.Instance.CreateLayer();    // HUD layer.

            Renderer.Instance.ShowLayer(MapLayer);
            Renderer.Instance.ShowLayer(EntityLayerBottom);
            Renderer.Instance.ShowLayer(EntityLayerMiddle);
            Renderer.Instance.ShowLayer(EntityLayerTop);
            Renderer.Instance.ShowLayer(EffectsLayer);
            Renderer.Instance.ShowLayer(HUDLayer);
        }
    }
}
