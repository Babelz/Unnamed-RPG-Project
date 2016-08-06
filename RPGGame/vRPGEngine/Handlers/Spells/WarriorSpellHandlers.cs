using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using vRPGContent.Data.Spells;
using vRPGEngine.Databases;
using vRPGEngine.ECS;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class BattleShout : SelfBuffSpellHandler
    {
        public BattleShout()
            : base("BattleShout", SpellDatabase.Instance.Elements().First(p => p.ID == 0), 3000, null)
        {
        }
    }
}
