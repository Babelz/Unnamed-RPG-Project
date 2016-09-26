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
        private const int Hp5Timer      = 0;
        private const int Mp5Timer      = 1;
        private const int Fp5Timer      = 2;

        private const int TimersCount   = 3;

        private const int TickTime      = 33;
        #endregion

        #region Fields
        private int[] timers;
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
            this.Controller = controller;

            timers          = new int[TimersCount];
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
            for (var i = 0; i < TimersCount; i++)
            {
                var elapsed = timers[i] + gameTime.ElapsedGameTime.Milliseconds;
                
                if (elapsed >= TickTime)
                {
                    var delta = (elapsed - TickTime) * 0.01f;

                    elapsed -= TickTime;
                    
                    switch (i)
                    {
                        case Hp5Timer: RegenHP((int)(Controller.Specialization.TotalHp5() * delta), "Hp5");        break;
                        case Mp5Timer: RegenMana((int)(Controller.Specialization.TotalMp5() * delta), "Mp5");      break;
                        case Fp5Timer: RegenFocus((int)(Controller.Specialization.TotalFp5() * delta), "Fp5");     break;
                        default:                                                                                             break;
                    }
                }

                timers[i] = elapsed;
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

            if (Controller.Statuses.Health + amount <= Controller.Specialization.TotalHealth()) GameInfoLog.Instance.LogGainHealth(amount, "Hp5", "Player");

            base.RegenHP(amount, from);    
        }
        public override void RegenMana(int amount, string from)
        {
            if (amount == 0) return;

            if (Controller.Statuses.Mana + amount <= Controller.Specialization.TotalMana()) GameInfoLog.Instance.LogGainMana(amount, "Mp5", "Player");

            base.RegenMana(amount, from);
        }
        public override void RegenFocus(int amount, string from)
        {
            if (amount == 0) return;

            if (Controller.Statuses.Focus + amount <= Controller.Specialization.TotalFocus()) GameInfoLog.Instance.LogGainFocus(amount, "Fp5", "Player");

            base.RegenFocus(amount, from);
        }
    }
}
