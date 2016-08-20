using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Spells;

namespace vRPGEngine.HUD.Controls
{
    public sealed class Icon : MouseControlBase
    {
        #region Fields
        //private IconElement element;
        private Spell spell;
        #endregion

        public Icon()
            : base()
        {
            PropertyChanged += Icon_PropertyChanged;
        }

        #region Event handlers
        private void Icon_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        #endregion

        protected override void OnInvalidate()
        {
            // if (element != null) element.Invalidate(this);
        }

        protected override void OnRender(GameTime gameTime)
        {
            
        }
    }
}
