using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using vRPGEngine.Core;

namespace vRPGEngine.Scenes
{
    public sealed class SceneManager : SystemManager<SceneManager>
    {
        #region Fields
        private bool sceneChanging;

        private Scene current;
        private Scene next;
        #endregion

        private SceneManager()
            : base()
        {
        }

        public void ChangeScene(Scene scene)
        {
            Debug.Assert(scene != null);
            Debug.Assert(!sceneChanging);

            sceneChanging   = true;
            next            = scene;

            next.Initialize();
        }

        protected override void OnUpdate(GameTime gameTime)
        {
            if (sceneChanging)
            {
                if (next.ChangeScene())
                {
                    if (current != null) current.Deinitialize();

                    sceneChanging   = false;
                    current         = next;
                    next            = null;
                }
            }

            if (current != null) current.Update(gameTime);
            if (next != null)    next.Update(gameTime);
        }
    }
}
