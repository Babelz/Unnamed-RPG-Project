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
        public const int Bottom  = 0;   // Map base.
        public const int Middle  = 1;   // Entity layer and map middle.
        public const int Top     = 2;   // Map top.
        public const int Effects = 3;  
        #endregion

        public static void Create()
        {
            if (Renderer.Instance.HasLayers()) Renderer.Instance.ClearLayers();

            Renderer.Instance.CreateLayer();
            Renderer.Instance.CreateLayer();
            Renderer.Instance.CreateLayer();
            Renderer.Instance.CreateLayer();

            Renderer.Instance.ShowLayer(Bottom);
            Renderer.Instance.ShowLayer(Middle);
            Renderer.Instance.ShowLayer(Top);
            Renderer.Instance.ShowLayer(Effects);
        }
    }
}
