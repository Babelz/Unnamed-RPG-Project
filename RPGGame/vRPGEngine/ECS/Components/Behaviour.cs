using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine.ECS.Components
{
    public class Behaviour : Component<Behaviour>
    {
        #region Properties
        public bool Behaving
        {
            get;
            set;
        }
        #endregion

        public Behaviour()
            : base()
        {
            Behaving = true;
        }
        
        protected virtual void Behave(GameTime gameTime)
        {
        }

        public void Update(GameTime gameTime)
        {
            if (!Behaving) return;

            Behave(gameTime);
        }
    }
}
