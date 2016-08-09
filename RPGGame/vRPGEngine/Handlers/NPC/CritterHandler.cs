using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using vRPGContent.Data.Characters;

namespace vRPGEngine.Handlers.NPC
{
    public sealed class CritterHandler : NPCHandler
    {
        public CritterHandler(NPCData data) 
            : base("CritterHandler", data)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
