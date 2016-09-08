using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class EngineSettings
    {
        #region Fields
        private readonly IniFile ini;
        #endregion

        #region Properties
        public int ResolutionWidth
        {
            get
            {
                return int.Parse(ini.Read("resolution_width", "graphics"));
            }
        }
        public int ResolutionHeight
        {
            get
            {
                return int.Parse(ini.Read("resolution_height", "graphics"));
            }
        }
        #endregion

        public EngineSettings()
        {
        }

        public void Load()
        {
        }
        public void Save()
        {
        }
    }

    public sealed class HUDSettings
    {
        #region Properties
        public bool Visible
        {
            get;
            set;
        }
        #endregion

        public HUDSettings()
        {
        }

        public void Load()
        {
        }
        public void Save()
        {
        }
    }

    public sealed class CombatTextSettings
    {
        #region Floating type
        public enum FloatingTextBehaviour
        {
            FromTopToBottom,
            FromBottomToTop,
            FlyToSides
        }
        #endregion

        #region Properties
        public FloatingTextBehaviour FloatingBehaviour
        {
            get;
            set;
        }
        public bool ShowDamageDealt
        {
            get;
            set;
        }
        public bool ShowDamageTaken
        {
            get;
            set;
        }
        public bool Visible
        {
            get;
            set;
        }
        public bool ShowReputationChanges
        {
            get;
            set;
        }
        public bool ShowHealthGains
        {
            get;
            set;
        }
        public bool ShowManaGains
        {
            get;
            set;
        }
        public bool ShowFocusGains
        {
            get;
            set;
        }
        #endregion

        static CombatTextSettings()
        {
        }

        public void Load()
        {
        }
        public void Save()
        {
        }
    }

    public static class GameSetting
    {
        #region Properties
        public static HUDSettings HUD
        {
            get;
            private set;
        }
        public static CombatTextSettings CombatText
        {
            get;
            private set;
        }
        #endregion

        static GameSetting()
        {
            HUD         = new HUDSettings()
            {
                Visible = true
            };

            CombatText  = new CombatTextSettings()
            {
                ShowDamageDealt         = true,
                ShowDamageTaken         = true,
                ShowFocusGains          = true,
                ShowHealthGains         = true,
                ShowManaGains           = true,
                ShowReputationChanges   = true,
                Visible                 = true,
                FloatingBehaviour       = CombatTextSettings.FloatingTextBehaviour.FromBottomToTop
            };
        }

        public static void Load()
        {
            HUD.Load();
            CombatText.Load();
        }
        public static void Save()
        {
            HUD.Save();
            CombatText.Save();
        }
    }
}
