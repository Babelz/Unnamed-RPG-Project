using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Core;
using vRPGEngine.Graphics;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    [Flags()]
    public enum ImagePresentationFlags : int
    {
        None = 0,
        FlipVertical,
        FlipHorizontal,
    }

    public sealed class Image : Control
    {
        #region Fields
        private ImagePresentationFlags presentationFlags;
        private IDisplayElement element;

        private Texture2D texture;

        private Vector2 imageOrigin;
        private Vector2 imageScale;

        private Rectangle source;
        private Color color;
        private float rotation;
        #endregion

        #region Properties
        public ImagePresentationFlags PresentationFlags
        {
            get
            {
                return presentationFlags;
            }
            set
            {
                presentationFlags = value;

                NotifyPropertyChanged("PresentationFlags");
            }
        }
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;

                NotifyPropertyChanged("Texture");
            }
        }
        public Rectangle Source
        {
            get
            {
                return source;
            }
            set
            {
                source = value;

                NotifyPropertyChanged("Source");
            }
        }
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;

                NotifyPropertyChanged("Rotation");
            }
        }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;

                NotifyPropertyChanged("Color");
            }
        }
        
        public Vector2 ImageOrigin
        {
            get
            {
                return imageOrigin;
            }
            set
            {
                imageOrigin = value;

                NotifyPropertyChanged("Origin");
            }
        }
        public Vector2 ImageScale
        {
            get
            {
                return imageScale;
            }
            set
            {
                imageScale = value;

                NotifyPropertyChanged("Scale");
            }
        }
        
        public IDisplayElement Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;

                NotifyPropertyChanged("Element");
            }
        }
        #endregion

        public Image()
            : base()
        {
            element = new ImageElement();

            RegisterProperty("PresentationFlags", () => PresentationFlags, (o) => presentationFlags = (ImagePresentationFlags)o);
            RegisterProperty("Texture", () => Texture, (o) => Texture = (Texture2D)o);
            RegisterProperty("Source", () => Source, (o) => Source = (Rectangle)o);
            RegisterProperty("Rotation", () => Rotation, (o) => Rotation = (float)o);
            RegisterProperty("Color", () => Color, (o) => Color = (Color)o);
            RegisterProperty("ImageOrigin", () => ImageOrigin, (o) => ImageOrigin = (Vector2)o);
            RegisterProperty("ImageScale", () => ImageScale, (o) => ImageScale = (Vector2)o);
            RegisterProperty("Element", () => Element, (o) => Element = (IDisplayElement)o);

            PropertyChanged += Image_PropertyChanged;
        }

        /*
                TODO: check all control invalidations as they are not optimized!
         */

        #region Properties
        private void Image_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        #endregion

        protected override void OnInvalidate()
        {
            if (Element != null) element.Invalidate(this);
        }

        protected override void OnDraw(GameTime gameTime)
        {
            if (Element != null) HUDRenderer.Instance.Show(element);
        }

        public void SetOriginCenter()
        {
            if (Texture == null)
            {
                Logger.Instance.LogWarning("SetOriginCenter had no effect, texture can't be null");

                return;
            }

            ImageOrigin = new Vector2(texture.Bounds.Width / 2.0f, texture.Bounds.Height / 2.0f);
        }
        public void SetOriginTopLeft()
        {
            ImageOrigin = Vector2.Zero;
        }
    }
}
