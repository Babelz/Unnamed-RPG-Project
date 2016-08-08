using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vRPGContent.Data.Characters.Enums
{
    [Flags()]
    public enum Factions : int
    {
        /// <summary>
        /// Not an actual faction, always hostile towards the player.
        /// </summary>
        WorldHostile = 1,

        /// <summary>
        /// Not an actual faction, always neutral towards the player.
        /// </summary>
        WorldNeutral = 2,
        
        /// <summary>
        /// Not and actual faction, always friendly towards the player.
        /// </summary>
        WorldFriendly = 4
    }
}
