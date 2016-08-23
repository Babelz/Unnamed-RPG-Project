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
        public static bool HUDVisible
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
        public static bool ShowDamageDealt
        {
            get;
            set;
        }
        public static bool ShowDamageTaken
        {
            get;
            set;
        }
        public static bool Visible
        {
            get;
            set;
        }
        public static bool ShowReputationChanges
        {
            get;
            set;
        }
        public static bool ShowHealthGains
        {
            get;
            set;
        }
        public static bool ShowManaGains
        {
            get;
            set;
        }
        public static bool ShowFocusGains
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
        #endregion

        static GameSetting()
        {
            HUDVisible = true;
        }

        public static void Load()
        {
        }
        public static void Save()
        {
        }
    }
}
