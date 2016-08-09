﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Characters;

namespace vRPGEngine.Handlers.NPC
{
    public sealed class NPCHandlerFactory : HandlerFactory<NPCHandlerFactory, NPCData, NPCHandler>
    {
        public NPCHandlerFactory() 
            : base("vRPGEngine.Handlers.NPC.")
        {
        }
    }
}
