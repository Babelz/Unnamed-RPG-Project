using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.HUD.Controls
{
    [Flags()]
    public enum ImagePresentationFlags : int
    {
        FlipVertical,
        FlipHorizontal,
    }

    public sealed class Image : Control
    {
        #region Fields
        private ImagePresentationFlags presentationFlags;

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
        #endregion

        public Image()
            : base()
        {
            RegisterProperty("PresentationFlags", () => PresentationFlags, (o) => presentationFlags = (ImagePresentationFlags)o);
        }
    }
}
