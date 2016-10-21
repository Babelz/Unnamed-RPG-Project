using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Core;
using vRPGEngine.HUD;
using vRPGEngine;

namespace RPGGame.GUI
{
    public sealed class FloatingHUDTextManager : HUDSubsystem
    {
        #region Combat text entry class
        private sealed class FloatingHUDTextEntry
        {
            public string  Contents;
            public bool    Bold;
            public Color   Color;
            public float   Alpha;
            public float   Angle;
            public Vector2 Position;
            public int     Side;
            
            public FloatingHUDTextEntry(string contents, Color color, bool bold = false)
            {
                Contents    = contents;
                Color       = color;
                Bold        = bold;
                Alpha       = 1.0f;
                Angle       = 0.0f;
                Side        = 0;
            }
        }
        #endregion
        
        #region Fields
        private readonly Func<InfoLogEntry, FloatingHUDTextEntry>[] formatters;

        private readonly List<FloatingHUDTextEntry> newEntries;
        private readonly List<FloatingHUDTextEntry> allEntries;
        
        private int side;
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get;
            set;
        }
        #endregion

        public FloatingHUDTextManager()
        {
            Font            = DefaultValues.DefaultFont;

            var critical    = "(critical)";

            formatters = new Func<InfoLogEntry, FloatingHUDTextEntry>[]
            {
                // Message
                null,
               
                // Warning
                (e) => new FloatingHUDTextEntry(e.Contents, Color.Yellow, false),

                // TakeDamage
                (e) => new FloatingHUDTextEntry(e.Data, Color.Red, e.Contents.Contains(critical)),
                
                // DealDamage
                (e) => new FloatingHUDTextEntry(e.Data, Color.White, e.Contents.Contains(critical)),
                
                // GainHealth
                (e) => new FloatingHUDTextEntry("Health +" + e.Data, Color.Green),
                
                // GainMana
                (e) => new FloatingHUDTextEntry("Mana +" + e.Data, Color.Blue),
                
                // GainFocus
                (e) => new FloatingHUDTextEntry("Focus +" + e.Data, Color.Yellow),
               
                // GainReputateon
                (e) => new FloatingHUDTextEntry("Reputation " + e.Data, Color.BlueViolet),
                
                // UseSpell
                (e) => new FloatingHUDTextEntry("Spell: " + e.Data, Color.Green),
                
                // GainBuff
                (e) => new FloatingHUDTextEntry("Gain buff: " + e.Data, Color.Green),
                
                // LoseBuff
                (e) => new FloatingHUDTextEntry("Lose buff: " + e.Data, Color.Red),
                
                // GainDebuff
                (e) => new FloatingHUDTextEntry("Gain debuff: " + e.Data, Color.Red),
                
                // LoseDebuff
                (e) => new FloatingHUDTextEntry("Lose debuff: " + e.Data, Color.Green),
            };

            newEntries = new List<FloatingHUDTextEntry>();
            allEntries = new List<FloatingHUDTextEntry>();

            Enable();
            Show();
        }
        
        private bool TextSideLeft()
        {
            return side % 2 == 0;
        }
        private bool TextSideRight()
        {
            return side % 2 != 0;
        }

        private void CreateNewEntries()
        {
            foreach (var logEntry in GameInfoLog.Instance.Entries())
            {
                var formatterIndex  = (int)logEntry.Type;
                var formatter       = formatters[formatterIndex];

                if (formatter == null) continue;

                newEntries.Add(formatter(logEntry));
            }
        }
        private void InitializeNewEntries()
        {
            var transform   = EntityManager.Instance.Entitites.First(e => e.Tagged("player")).FirstComponentOfType<Transform>();
            var position    = transform.Position;

            foreach (var newEntry in newEntries)
            {
                if (TextSideLeft()) { position.X -= Font.MeasureString(newEntry.Contents).X; newEntry.Side = -1; }
                else                { position.X += Font.MeasureString(newEntry.Contents).X / 2.0f; newEntry.Side = 1;  }

                if (GameSetting.CombatText.FloatingBehaviour != CombatTextSettings.FloatingTextBehaviour.FlyToSides)
                {
                    position.Y        += Font.MeasureString(newEntry.Contents).Y;

                    newEntry.Position = position;
                }

                allEntries.Add(newEntry);

                side++;
            }

            newEntries.Clear();
        }
        
        private void UpdatEntries(GameTime gameTime)
        {
            var dt       = gameTime.ElapsedGameTime.Milliseconds * 0.1f;
            var velocity = 0.35f;

            for (int i = 0; i < allEntries.Count; i++)
            {
                var entry = allEntries[i];

                switch (GameSetting.CombatText.FloatingBehaviour)
                {
                    case CombatTextSettings.FloatingTextBehaviour.FromTopToBottom:
                        entry.Position.Y += velocity * dt;
                        break;
                    case CombatTextSettings.FloatingTextBehaviour.FromBottomToTop:
                        entry.Position.Y -= velocity * dt;
                        break;
                    case CombatTextSettings.FloatingTextBehaviour.FlyToSides:
                        var radius = Font.MeasureString(entry.Contents).X;

                        var cx = HUDRenderer.Instance.CanvasSize.X / 2.0f + (float)Math.Cos(entry.Angle) * radius * dt * entry.Side;
                        var cy = HUDRenderer.Instance.CanvasSize.Y / 2.0f + (float)Math.Sin(entry.Angle) * radius * dt * entry.Side;

                        entry.Position.X = cx;
                        entry.Position.Y = cy;
                        break;
                    default:
                        break;
                }

                entry.Alpha -= 0.0025f * dt;
                entry.Angle += 0.025f * dt;

                if (entry.Alpha < 0.0f) allEntries.RemoveAt(i);
            }
        }

        protected override void OnUpdate(GameTime gameTime, SpriteBatch spriteBatch)
        {
            CreateNewEntries();

            InitializeNewEntries();

            UpdatEntries(gameTime);
        }
        protected override void OnDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var entry in allEntries)
            {
                spriteBatch.DrawString(Font,
                                       entry.Contents,
                                       entry.Position,
                                       entry.Color * entry.Alpha,
                                       0.0f,
                                       Vector2.Zero,
                                       entry.Bold ? 1.5f : 1.0f,
                                       SpriteEffects.None,
                                       0.0f);
            }
        }
    }
}
