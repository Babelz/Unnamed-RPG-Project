using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.HUD.Elements;
using vRPGEngine.Input;
using Microsoft.Xna.Framework;
using vRPGEngine.Handlers.Spells;

namespace vRPGEngine.HUD.Controls
{
    public sealed class BindButton : ButtonBase
    {
        #region Fields
        private readonly string name;

        private IDisplayElement element;

        private SpellHandler handler;

        private Spell spell;
        #endregion

        #region Properties
        public IDisplayElement Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;

                NotifyPropertyChanged("Spell");
            }
        }

        public Spell Spell
        {
            get
            {
                return spell;
            }
            set
            {
                spell = value;

                NotifyPropertyChanged("Spell");
            }
        }

        private SpellHandler Handler
        {
            get
            {
                return handler;
            }
            set
            {
                handler = value;

                NotifyPropertyChanged("Handler");
            }
        }
        #endregion

        public BindButton(string name)
            : base()
        {
            this.name = name;

            ButtonPressed   += BindButton_ButtonPressed;
            PropertyChanged += BindButton_PropertyChanged;
        }

        #region Event handlers
        private void BindButton_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();
        }
        private void BindButton_ButtonPressed(Interfaces.IButtonControl sender)
        {
            Use();
        }
        #endregion

        protected override void OnInvalidate()
        {
            var player = EntityManager.Instance.Entitites.First(e => e.Tags == "player");

            var controller = player.FirstComponentOfType<PlayerCharacterController>();

            if (spell != null) Handler = controller.Spells.FirstOrDefault(p => p.Spell.ID == spell.ID);
            
            if (element != null) element.Invalidate(this);
        }

        private void Use()
        {
            var player = EntityManager.Instance.Entitites.First(e => e.Tags == "player");
            
            if (Handler != null) Handler.Use(player);
        }

        public void SetTrigger(Keys key)
        {
            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();

            kb.Unbind(name);

            kb.Bind(name, key, KeyTrigger.Pressed, () =>
            {
                Use();
            });

            Invalidate();
        }
    }
}
