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
using vRPGEngine.Handlers.Spells;
using vRPGEngine;

namespace RPGGame.Handlers.Spells
{
    public sealed class BattleShout : SelfBuffSpellHandler
    {
        #region Private buff class
        private sealed class BattleShoutBuff : Buff
        {
            #region Constants
            private const float BuffValue = 0.05f;
            #endregion

            public BattleShoutBuff() 
                : base(0, false, false, SpellDatabase.Instance.Elements().First(p => p.ID == 0), BuffType.Buff)
            {
            }

            public override void Apply(Entity user)
            {
                var character = user.FirstComponentOfType<ICharacterController>();

                character.Attributes.HealthPercentModifier      += BuffValue;
                character.Attributes.AttackPowerPercentModifier += BuffValue;
            }   
            public override void Remove(Entity user)
            {
                var character = user.FirstComponentOfType<ICharacterController>();

                character.Attributes.HealthPercentModifier      -= BuffValue;
                character.Attributes.AttackPowerPercentModifier -= BuffValue;
            }
        }
        #endregion
        
        public BattleShout()
            : base("battle shout", TimeConverter.ToMilliseconds(30.0f), null)
        {
        }

        protected override bool RefreshIfCan(Buff buff)
        {
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell)) return false;

            buff.Refresh();

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);

            return true;
        }
        protected override Buff UseIfCan()
        {
            if (!SpellHelper.CanUse(UserController.Specialization, UserController.Statuses, Spell)) return null;

            var buff = new BattleShoutBuff();

            buff.Apply(User);

            UserController.Buffs.Add(buff);

            SpellHelper.ConsumeCurrencies(UserController.Specialization, UserController.Statuses, Spell);

            return buff;
        }
        
        public override SpellHandler Clone()
        {
            return new BattleShout();
        }
    }
}
