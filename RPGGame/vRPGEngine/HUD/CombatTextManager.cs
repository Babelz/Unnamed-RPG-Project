using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace vRPGEngine.HUD
{
    public sealed class CombatTextManager
    {
        #region CombatTextEntry struct
        private struct CombatTextEntry
        {
            public string Contents;
            public Color  Color;
            public float  Alpha;
            public float  Angle;
        }
        #endregion

        #region Fields
        private readonly List<CombatTextEntry> entries;
        #endregion

        public CombatTextManager()
        {
            entries = new List<CombatTextEntry>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var logEntry in GameInfoLog.Instance.Entries())
            {

            }

            GameInfoLog.Instance.Clear();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
