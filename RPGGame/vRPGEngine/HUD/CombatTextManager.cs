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
            public string  Contents;
            public Color   Color;
            public float   Alpha;
            public float   Angle;
            public Vector2 Position;
            
            public static CombatTextEntry Create(string contents, Color color)
            {
                CombatTextEntry entry;

                entry.Contents  = contents;
                entry.Color     = color;
                entry.Alpha     = 1.0f;
                entry.Angle     = 0.0f;
                entry.Position  = Vector2.Zero;

                return entry;
            }
        }
        #endregion

        #region Fields
        private readonly Func<InfoLogEntry, CombatTextEntry>[] formatters;

        private readonly List<CombatTextEntry> entries;
        #endregion

        public CombatTextManager()
        {
            formatters = new Func<InfoLogEntry, CombatTextEntry>[]
            {
                // Message
                null,
               
                // Warning
                null,

                // TakeDamage
                (i) => CombatTextEntry.Create(i.Contents, Color.DarkRed),
                
                // DealDamage
                (i) => CombatTextEntry.Create(i.Contents, Color.Red),
                
                // GainHealth
                (i) => CombatTextEntry.Create(i.Contents, Color.Green),
                
                // GainMana
                (i) => CombatTextEntry.Create(i.Contents, Color.Blue),
                
                // GainFocus
                (i) => CombatTextEntry.Create(i.Contents, Color.GreenYellow),
               
                // GainReputation
                (i) => CombatTextEntry.Create(i.Contents, Color.BlueViolet),
                
                // UseSpell
                (i) => CombatTextEntry.Create(i.Contents, Color.Red),
                
                // GainBuff
                (i) => CombatTextEntry.Create(i.Contents, Color.Green),
                
                // LoseBuff
                (i) => CombatTextEntry.Create(i.Contents, Color.Green),
                
                // GainDebuff
                (i) => CombatTextEntry.Create(i.Contents, Color.Red),
                
                // LoseDebuff
                (i) => CombatTextEntry.Create("Lose buff, Color.Red),
            };

            entries = new List<CombatTextEntry>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var logEntry in GameInfoLog.Instance.Entries())
            {
                switch (logEntry.Type)
                {
                    case InfoLogEntryType.Message:
                        break;
                    case InfoLogEntryType.Warning:
                        break;
                    case InfoLogEntryType.TakeDamage:
                        break;
                    case InfoLogEntryType.DealDamage:
                        break;
                    case InfoLogEntryType.GainHealth:
                        break;
                    case InfoLogEntryType.GainMana:
                        break;
                    case InfoLogEntryType.GainFocus:
                        break;
                    case InfoLogEntryType.GainReputation:
                        break;
                    case InfoLogEntryType.UseSpell:
                        break;
                    case InfoLogEntryType.GainBuff:
                        break;
                    case InfoLogEntryType.LoseBuff:
                        break;
                    case InfoLogEntryType.GainDebuff:
                        break;
                    case InfoLogEntryType.LoseDebuff:
                        break;
                    default:
                        break;
                }
            }

            GameInfoLog.Instance.Clear();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
