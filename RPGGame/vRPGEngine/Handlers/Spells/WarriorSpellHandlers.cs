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
using vRPGEngine.Attributes.Spells;
using vRPGEngine.Attributes.Enums;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Handlers.Spells
{
    public sealed class BattleShout : SelfBuffSpellHandler
    {
        #region Private buff class
        private sealed class BattleShoutBuff : Buff
        {
            #region Constants
            private const float Value = 0.05f;
            #endregion

            public BattleShoutBuff() 
                : base("Battle shout", 3000, false, false, SpellDatabase.Instance.Elements().First(p => p.ID == 0), BuffType.Buff)
            {
            }

            public override void Apply(Entity owner)
            {
                var character = owner.FirstComponentOfType<CharacterController>();

                character.Attributes.HealthPercentModifier      += Value;
                character.Attributes.MeeleePowerPercentModifier += Value;
            }
            public override void Remove(Entity owner)
            {
                var character = owner.FirstComponentOfType<CharacterController>();

                character.Attributes.HealthPercentModifier      -= Value;
                character.Attributes.MeeleePowerPercentModifier -= Value;
            }
        }
        #endregion
        
        public BattleShout()
            : base("BattleShout", SpellDatabase.Instance.Elements().First(p => p.ID == 0), 3000, null)
        {
        }
    }
}
