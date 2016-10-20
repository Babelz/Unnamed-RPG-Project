using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vRPGContent.Data.Spells;
using vRPGEngine.Attributes.Spells;
using vRPGEngine.Combat;
using vRPGEngine.Core;
using vRPGEngine.Databases;
using vRPGEngine.ECS;
using vRPGEngine.ECS.Components;
using vRPGEngine.Graphics;
using vRPGEngine.Interfaces;

namespace vRPGEngine.Handlers.Spells
{
    public abstract class SpellHandler
    {
    }

    public abstract class MeleeSpellHandler
    {
    }

    public abstract class MissileSpellHandler
    {
    }

    public abstract class InstantRangedSpellHandler
    {
    }

    public abstract class BuffSpellHandler
    {
    }

    public abstract class AOESpellHandler
    {
    }
}