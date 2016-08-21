using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.HUD.Elements;

namespace vRPGEngine.HUD.Controls
{
    public sealed class BindButton : ButtonBase
    {
        #region Fields
        private readonly BindButtonElement button;

        private readonly IconElement icon;

        private object content;
        private Keys keys;
        #endregion

        #region Properties
        public object Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;

                NotifyPropertyChanged("Content");
            }
        }

        public Keys Keys
        {
            get
            {
                return keys;
            }

            set
            {
                keys = value;

                NotifyPropertyChanged("Keys");
            }
        }
        #endregion

        public BindButton()
            : base()
        {
        }
        
        public void Use()
        {
        }
    }
}
