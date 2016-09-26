using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGEngine.Commands;
using vRPGEngine.Core;
using vRPGEngine.ECS;
using vRPGEngine.Graphics;
using vRPGEngine.Handlers.Spells;
using vRPGEngine.HUD.Elements;
using vRPGEngine.Input;

namespace vRPGEngine.HUD.Controls
{
    public sealed class BindButton : ButtonBase
    {
        #region Bind button commands
        private sealed class SpellCommand : ICommand
        {
            #region Fields
            private readonly SpellHandler handler;
            #endregion

            public SpellCommand(SpellHandler handler)
                : base()
            {
                Debug.Assert(handler != null);

                this.handler = handler;
            }
            
            public void Execute()
            {
                handler.Use(EntityManager.Instance.Entitites.FirstOrDefault(e => e.Tagged("player")));
            }
        }
        #endregion

        #region Static fields
        private static int idc;
        #endregion
        
        #region Fields
        private readonly string bindingName;

        private ICommand command;
        private IDisplayElement element;

        private SpriteFont font;
        private object content;

        private Keys keys;
        #endregion

        #region Properties
        public Keys Keys
        {
            get
            {
                return keys;
            }

            set
            {
                keys = value;

                NotifyPropertyChanged("Keys");

                Rebind();
            }
        }

        public object Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;

                NotifyPropertyChanged("Content");
            }
        }
        public SpriteFont Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;

                NotifyPropertyChanged("Font");
            }
        }
        public IDisplayElement Element
        {
            get
            {
                return element;
            }
            set
            {
                element = value;

                NotifyPropertyChanged("Element");
            }
        }
        #endregion

        public BindButton()
            : base()
        {
            bindingName = string.Format("{0}:{1}", Name, idc++);
            font        = DefaultValues.DefaultFont;

            RegisterProperty("Element", () => Element, (o) => Element = (IDisplayElement)o);
            RegisterProperty("Content", () => Content, (o) => Content = o);
            RegisterProperty("Font", () => Font, (o) => Font = (SpriteFont)o);

            PropertyChanged += BindButton_PropertyChanged;
            ButtonPressed   += BindButton_ButtonPressed;
        }

        #region Event handlers
        private void BindButton_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            Invalidate();

            if (args.PropertyName == "Content") InvalidateCommand();
        }
        private void BindButton_ButtonPressed(Interfaces.IButtonControl sender)
        {
            Use();
        }
        #endregion
        
        protected override void OnInvalidate()
        {
             if (element != null) element.Invalidate(this);
        }
        protected override void OnDraw(GameTime gameTime)
        {
            if (element != null) HUDRenderer.Instance.Show(element);
        }

        private void InvalidateCommand()
        {
            if (content == null)
            {
                command = null;

                return;
            }

            var spell = content as SpellHandler;

            if (spell != null) command = new SpellCommand(spell);
        }

        private void Use()
        {
            if (command == null) return;

            command.Execute();
        }

        private void Rebind()
        {
            var kb = InputManager.Instance.GetProvider<KeyboardInputProvider>();

            kb.Unbind(bindingName);
            kb.Bind(bindingName, Keys, KeyTrigger.Pressed, Use);
        }
    }
}
