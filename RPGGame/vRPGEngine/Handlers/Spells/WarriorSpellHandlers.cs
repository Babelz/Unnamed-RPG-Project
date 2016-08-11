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
                : base(TimeConverter.ToMilliseconds(30.0f), false, false, SpellDatabase.Instance.Elements().First(p => p.ID == 0), BuffType.Buff)
            {
            }

            public override void Apply(Entity user)
            {
                var character = user.FirstComponentOfType<ICharacterController>();

                character.Attributes.HealthPercentModifier      += Value;
                character.Attributes.MeeleePowerPercentModifier += Value;
            }
            public override void Remove(Entity user)
            {
                var character = user.FirstComponentOfType<ICharacterController>();

                character.Attributes.HealthPercentModifier      -= Value;
                character.Attributes.MeeleePowerPercentModifier -= Value;
            }
        }
        #endregion
        
        public BattleShout()
            : base(SpellDatabase.Instance.Elements().First(p => p.ID == 0), TimeConverter.ToMilliseconds(30.0f), null)
        {
        }

        protected override void RefreshIfCan(Buff buff)
        {
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell)) return;

            buff.Refresh();

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);
        }
        protected override void UseIfCan()
        {
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell)) return;

            var buff = new BattleShoutBuff();

            buff.Apply(User);

            UserController.Buffs.Add(buff);

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);
        }
        
        public override object Clone()
        {
            return new BattleShout();
        }
    }
}
