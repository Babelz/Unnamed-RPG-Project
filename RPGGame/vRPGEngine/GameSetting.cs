using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGEngine
{
    public sealed class HUDSettings
    {
        #region Properties
        public bool HUDVisible
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
        #region Properties
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
                HUDVisible = true
            };

            CombatText  = new CombatTextSettings()
            {
                ShowDamageDealt         = true,
                ShowDamageTaken         = true,
                ShowFocusGains          = true,
                ShowHealthGains         = true,
                ShowManaGains           = true,
                ShowReputationChanges   = true,
                Visible                 = true
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
