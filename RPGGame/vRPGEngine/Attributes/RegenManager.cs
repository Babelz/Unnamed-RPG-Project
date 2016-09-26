using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.ECS.Components;

namespace vRPGEngine.Attributes
{
    public class RegenManager
    {
        #region Constant fields
        private const int Hp5 = 0;
        private const int Mp5 = 1;
        private const int Fp5 = 2;

        private const int ValuesCount = 3;
        #endregion

        #region Fields
        private float[] values;
        #endregion

        #region Properties
        protected ICharacterController Controller
        {
            get;
            private set;
        }
        #endregion

        public RegenManager(ICharacterController controller)
        {
            Controller = controller;

            values = new float[ValuesCount];
        }

        public virtual void RegenHP(int amount, string from)
        {
            if (amount == 0) return;

            Controller.Statuses.Focus = MathHelper.Clamp(Controller.Statuses.Health + amount, 0, Controller.Specialization.TotalHealth());
        }
        public virtual void RegenMana(int amount, string from)
        {
            if (amount == 0) return;

            Controller.Statuses.Focus = MathHelper.Clamp(Controller.Statuses.Mana + amount, 0, Controller.Specialization.TotalMana());
        }
        public virtual void RegenFocus(int amount, string from)
        {
            if (amount == 0) return;

            Controller.Statuses.Focus = MathHelper.Clamp(Controller.Statuses.Focus + amount, 0, Controller.Specialization.TotalFocus());
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < ValuesCount; i++)
            {   
                    switch (i)
                    {
                    case Hp5:
                        values[Fp5] += Controller.Specialization.TotalHp5() / (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f;

                        if (values[Hp5] >= 1.0f)
                        {
                            var regen   = (int)values[Hp5];
                            values[Hp5] -= regen;

                            RegenHP(regen, "Hp5");
                        }
                        break;
                    case Mp5:
                        values[Mp5] += Controller.Specialization.TotalMp5() / (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f;

                        if (values[Mp5] >= 1.0f)
                        {
                            var regen   = (int)values[Mp5];
                            values[Mp5] -= regen;

                            RegenMana(regen, "Mp5");
                        }
                        break;
                    case Fp5:
                        values[Fp5] += Controller.Specialization.TotalFp5() / (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.1f;

                        if (values[Fp5] >= 1.0f)
                        {
                            var regen = (int)values[Fp5];
                            values[Fp5] -= regen;

                            RegenFocus(regen, "Fp5");
                        }
                        break;
                    default:
                        break;
                    }
            }
        }
    }

    public sealed class PlayerRegenManager : RegenManager
    {
        public PlayerRegenManager(ICharacterController controller)
            : base(controller)
        {
        }

        public override void RegenHP(int amount, string from)
        {
            if (amount == 0) return;

            //if (Controller.Statuses.Health + amount <= Controller.Specialization.TotalHealth()) GameInfoLog.Instance.LogGainHealth(amount, "Hp5", "Player");

            base.RegenHP(amount, from);    
        }
        public override void RegenMana(int amount, string from)
        {
            if (amount == 0) return;

            //if (Controller.Statuses.Mana + amount <= Controller.Specialization.TotalMana()) GameInfoLog.Instance.LogGainMana(amount, "Mp5", "Player");

            base.RegenMana(amount, from);
        }
        public override void RegenFocus(int amount, string from)
        {
            if (amount == 0) return;

            //if (Controller.Statuses.Focus + amount <= Controller.Specialization.TotalFocus()) GameInfoLog.Instance.LogGainFocus(amount, "Fp5", "Player");

            base.RegenFocus(amount, from);
        }
    }
}
