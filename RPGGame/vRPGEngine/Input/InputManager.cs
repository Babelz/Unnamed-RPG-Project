using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace vRPGEngine.Input
{
    public sealed class InputManager : SystemManager<InputManager>
    {
        #region Fields
        private readonly IInputProvider[] providers;
        #endregion

        private InputManager()
            : base("input manager")
        {
            providers = new IInputProvider[]
            {
                new KeyboardInputProvider(), // Keyboard.
                                             // Mouse.
            };
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            for (var i = 0; i < providers.Length; i++) providers[i].Update(gameTime);
        }

        public T GetProvider<T>() where T : class, IInputProvider
        {
            return providers.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }
    }
}
